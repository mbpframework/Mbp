using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EMS.Application.Utility.Excel
{
    /// <summary>
    /// Excel模板类
    /// </summary>
    public class PowerExcel
    {
        /// <summary>
        /// Excel对象
        /// </summary>
        private XSSFWorkbook _workbook;

        /// <summary>
        /// 工作簿
        /// </summary>
        private ISheet _sheet;

        /// <summary>
        /// 数据结束行
        /// </summary>
        private int _rowEnd = -1;

        /// <summary>
        /// 模板配置信息
        /// </summary>
        private PowerExcelConfig _config;

        /// <summary>
        /// 填充域sheet名称
        /// </summary>
        private static readonly string _fillSheet = "__fill";

        /// <summary>
        /// 列表sheet名称
        /// </summary>
        private static readonly string _listSheet = "__list";

        /// <summary>
        /// 数据填充Table
        /// </summary>
        private DataTable _dtFill;

        /// <summary>
        /// 列表数据Table
        /// </summary>
        private DataTable _dtList;

        /// <summary>
        /// 格式化
        /// </summary>
        private XSSFDataFormat _df;

        /// <summary>
        /// 下拉框数据源规则
        /// </summary>
        private List<PowerExcelRule> _rules = new List<PowerExcelRule>();

        /// <summary>
        /// 错误列表
        /// </summary>
        private List<ErrorDTO> _errors = new List<ErrorDTO>();

        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="s">文件</param>
        public PowerExcel(Stream s)
        {
            try
            {
                _workbook = new XSSFWorkbook(s);
                _sheet = _workbook.GetSheetAt(0);
                _df = new XSSFDataFormat(_workbook.GetStylesSource());
            }
            catch (Exception) //检测模板异常
            {
                throw new Exception("模板不正确");
            }

        }
        /// <summary>
        /// 构造方法 强行切换二开文件
        /// </summary>
        /// <param name="filePath">模板文件路径</param>
        public PowerExcel(string filePath)
        {
            //如果存在二开文件，切换成二开模式
            //todo: 这里还应该去获取是否是二开模式，暂时不知道怎么去获取，就先这样判断
            var xFileName = filePath.Replace(Path.GetFileName(filePath), "x_" + Path.GetFileName(filePath));
            if (File.Exists(xFileName))
            {
                filePath = xFileName;
            }
            if (File.Exists(filePath))
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }
            var fs = new FileStream(filePath, FileMode.Open);
            _workbook = new XSSFWorkbook(fs);
            _sheet = _workbook.GetSheetAt(0);
            _df = new XSSFDataFormat(_workbook.GetStylesSource());
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 渲染填充数据
        /// </summary>
        private void RenderExternalData()
        {
            if (_dtFill == null && string.IsNullOrEmpty(_config.Prop.FillSql) == false)
            {
                //_dtFill = CPQuery.From(_config.Prop.FillSql).ToDataTable();
            }

            var dr = _dtFill != null && _dtFill.Rows.Count > 0 ? _dtFill.Rows[0] : null;

            //遍历所有数据域字段
            foreach (var field in _config.Fill)
            {
                //获取数据行
                var row = _sheet.GetRow(field.RowIndex);
                if (row == null)
                {
                    continue;
                }

                //如果字段有公式数据，目前需要在渲染数据后才能设置，因为要获取数据总行数
                if (string.IsNullOrEmpty(field.Formula))
                {
                    var cell = row.GetCell(field.ColumnIndex) ?? row.CreateCell(field.ColumnIndex);

                    SetCellValue(cell, field, 0, dr != null && _dtFill.Columns.Contains(field.Field) ? dr[field.Field].ToString() : string.Empty);
                }
                else
                {
                    //起始行等于结束行 表示没有数据 该字公式就替换为空
                    if (_config.Prop.StartRow == _rowEnd)
                    {
                        //如果没有数据，公式包含了列表字段，直接将公式置空
                        if (field.Formula.Contains("$"))
                        {
                            field.Formula = string.Empty;
                        }
                    }

                    var cell = row.GetCell(field.ColumnIndex) ?? row.CreateCell(field.ColumnIndex);
                    //单列汇总
                    var formula = Regex.Replace(field.Formula, @"\$(?<name>\w+)", (match) =>
                    {
                        //获取字段名
                        var name = match.Groups["name"]?.Value;
                        if (string.IsNullOrEmpty(name))
                        {
                            return match.Value;
                        }
                        //查找字段信息
                        var index = _config.Row.First(t => t.Field == name);
                        var c = index.GetColumnChar();

                        //生成公式文本信息
                        return $"{c}{_config.Prop.StartRow}:{c}{_rowEnd - 1}";
                    });

                    //公式不为空 才设置
                    if (string.IsNullOrEmpty(formula))
                    {
                        cell.SetCellType(CellType.Numeric);
                        cell.SetCellValue(0);
                    }
                    else
                    {
                        cell.SetCellFormula(formula);
                        cell.SetCellType(CellType.Formula);
                    }
                }

            }

        }

        /// <summary>
        /// 渲染列表
        /// </summary>
        private void RenderList()
        {

            //获取列表区域起始行列号
            var rowIndex = _config.Prop.StartRow;
            var colIndex = _config.Prop.StartColumnIndex;

            //获取列表区域数据行和结束行号
            var _dataCount = _dtList.Rows.Count;
            _rowEnd = rowIndex + _dataCount;

            if (_dataCount > 0)
            {
                //整体移动数据下方的行，以保证留出可用的数据行出来 填充数据
                if (_sheet.LastRowNum > rowIndex + 1)
                {
                    _sheet.ShiftRows(rowIndex, _sheet.LastRowNum, _dataCount);
                }
                else
                {
                    _sheet.ShiftRows(rowIndex + 1, rowIndex + 100, _dataCount);
                }
            }

            //demo行 所有的数据行样式都会继承该行，后面插入数据，是拷贝该行然后重写数据
            var demoRow = _sheet.GetRow(_config.Prop.DemoRow);

            //插入列表域
            for (var i = 0; i < _dtList.Rows.Count; i++)
            {
                var drData = _dtList.Rows[i];

                //拷贝demo行
                //var row = demoRow.CopyRowTo(rowIndex);
                var row = _sheet.CreateRow(rowIndex);

                var _colIndex = colIndex;

                //显示序号，默认在起始列的前一列插入序号
                if (_config.Prop.ShowIndex)
                {
                    var cell = row.GetCell(_colIndex - 1) ?? row.CreateCell(_colIndex - 1);
                    cell.SetCellValue(i + 1);
                }

                //设置字段数据
                foreach (var fieldItem in _config.Row)
                {
                    var cell = row.GetCell(_colIndex) ?? row.CreateCell(_colIndex);
                    var cellVal = drData[fieldItem.Field].ToString();

                    SetCellValue(cell, fieldItem, rowIndex, cellVal);

                    _colIndex++;
                }

                if (demoRow != null)
                {
                    //设置单元格的样式
                    for (int index = 0; index < demoRow.Cells.Count; index++)
                    {
                        var rowCell = row.GetCell(index);
                        var demoCell = demoRow.Cells[index];
                        if (demoCell != null && rowCell != null)
                        {
                            rowCell.CellStyle = demoCell.CellStyle;
                        }
                    }
                }
                rowIndex++;
            }

            //删除demo行 
            //删除的逻辑 是从demo行下方开始，所有数据整体向上移动一行
            _sheet.ShiftRows(_config.Prop.StartRow, _sheet.LastRowNum < _config.Prop.StartRow ? _config.Prop.StartRow + 1 : _sheet.LastRowNum, -1);
        }

        /// <summary>
        /// 保存前调用
        /// </summary>
        private void OnBeforeSave()
        {
            //渲染列表
            RenderList();
            //渲染填充数据
            //RenderExternalData();
            ////缓存填充数据
            //CacheData(_dtFill, _fillSheet);
            //缓存列表数据
            //CacheData(_dtList, _listSheet);

            //应用规则
            //ApplyRule();

            //隐藏不显示的列
            HiddenColumn();

            _sheet.ForceFormulaRecalculation = true;//设置公式计算

            //非调试且设置了密码，则保护
            if (_config.Prop.Debug == false)
            {
                _sheet.ProtectSheet(_config.Prop.Password);
            }
        }

        /// <summary>
        /// 隐藏配置为不显示的列
        /// </summary>
        private void HiddenColumn()
        {
            foreach (var row in _config.Row)
            {
                if (string.IsNullOrEmpty(row.IsHidden))
                {
                    continue;
                }
                row.IsHidden = Regex.Replace(row.IsHidden, @"(?<type>\${1,2})(?<field>\w+)", (match) =>
                {
                    if (match.Groups["type"].Value == "$$" && _dtFill != null && _dtFill.Rows.Count > 0)
                        return _dtFill.Rows[0][match.Groups["field"].Value].ToString();
                    return string.Empty;
                });
                if (row.IsHidden == "0")
                {
                    //_sheet.SetColumnWidth(row.ColumnIndex, 0);
                    _sheet.SetColumnHidden(row.ColumnIndex, true);
                }

            }
        }

        /// <summary>
        /// 缓存某个表
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="sheetName">表格名称</param>
        private void CacheData(DataTable dt, string sheetName)
        {
            if (dt == null) return;
            //新建sheet
            var sheet = _workbook.CreateSheet(sheetName);//名为ref的工作表  
            var index = _workbook.GetSheetIndex(sheet);
            var row = sheet.CreateRow(0);
            ICell cell;
            //写入表头
            var colIndex = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                cell = row.CreateCell(colIndex++);
                cell.SetCellValue(dc.ColumnName);
                //设置单元格为字符串类型，便于取值
                cell.SetCellType(CellType.String);
            }
            //写入数据行
            var rowIndex = 1;

            //循环赋值
            foreach (DataRow dr in dt.Rows)
            {
                row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
                colIndex = 0;
                foreach (DataColumn dc in dt.Columns)
                {
                    cell = row.CreateCell(colIndex++);
                    cell.SetCellValue(dr[dc.ColumnName].ToString());

                    //设置单元格为字符串类型，便于取值
                    cell.SetCellType(CellType.String);
                }

                rowIndex++;
            }

            //隐藏缓存数据的列
            //调试模式下，显示该Excel
            _workbook.SetSheetHidden(index, _config.Prop.Debug ? false : true);
        }

        /// <summary>
        /// 获取缓存中的某个表
        /// </summary>
        /// <param name="sheetName">数据表</param>
        /// <param name="mergetList">要合并的数据表</param>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetCacheData(string sheetName, List<Dictionary<string, object>> mergetList)
        {
            //如果没有缓存，或者缓存数据为空，不需要合并
            var sheet = _workbook.GetSheet(sheetName);//名为ref的工作表  
            if (sheet == null)
            {
                return mergetList;
            }
            var rowCount = sheet.LastRowNum;
            if (rowCount == 0)
            {
                return mergetList;
            }

            //定义数据存储
            var list = new List<Dictionary<string, object>>();

            var headers = new List<string>();

            //获取表头
            var row = sheet.GetRow(0);
            foreach (var cell in row.Cells)
            {
                headers.Add(cell.StringCellValue);
            }

            //获取数据 
            //仅仅合并有数据的行
            for (var i = 0; i < mergetList.Count; i++)
            {
                //获取缓存数据行
                row = sheet.GetRow(i + 1);

                var dic = new Dictionary<string, object>();
                //读取缓存数据
                var colIndex = 0;
                foreach (var colName in headers)
                {
                    if (row == null)
                    {
                        dic[colName] = string.Empty;
                    }
                    else
                    {
                        var cell = row.GetCell(colIndex++);
                        dic[colName] = cell.StringCellValue;
                    }
                }

                //更新填报的数据
                var mergeDic = mergetList[i];
                foreach (var key in mergeDic.Keys)
                {
                    dic[key] = mergeDic[key];
                }

                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// 设置单元格的值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="filed">字段</param>
        /// <param name="dataRowIndex">行号 用于设置公式</param>
        /// <param name="cellVal">单元格值</param>
        private void SetCellValue(ICell cell, PowerExcelConfigCell filed, int dataRowIndex, string cellVal)
        {
            //设置单元格类型
            //如果没有公式，且指定了单元格类型，直接设置类型
            var cellType = filed.GetCellType();
            if (string.IsNullOrEmpty(filed.Formula) && cellType != CellType.Unknown)
            {
                cell.SetCellType(cellType);
            }
            else
            {
                cell.SetCellType(CellType.Formula);
            }
            //数值列，尝试将数据转换为数值
            if (cell.CellType == CellType.Numeric)
            {
                //获取格式信息
                string formatCode = _df.GetFormat(cell.CellStyle.DataFormat);

                //是否是日期列
                if (DateUtil.IsCellDateFormatted(cell))
                {
                    var val = cellVal.AsDateTime();
                    if (val != DateTime.MinValue)
                    {
                        cell.SetCellValue(val);
                    }
                }
                else
                {
                    // 否则就是其他数值列类型
                    double num = cellVal.AsDouble(double.MinValue);
                    if (num == double.MinValue)
                    {
                        //转换失败，直接赋值
                        cell.SetCellValue(cellVal);
                    }
                    else
                    {
                        //检测是否是百分比 是的话，手动除以100，因为程序里通常存的是50，表示50%
                        if (formatCode.EndsWith("%"))
                        {
                            cell.SetCellValue(num / 100);
                        }
                        else
                        {
                            cell.SetCellValue(num);
                        }
                    }
                }
            }
            else if (cell.CellType == CellType.Formula)
            {
                //公式列信息， {i} 标示当前行
                cell.CellFormula = filed.Formula.Replace("{i}", (dataRowIndex + 1).ToString());
            }
            else
            {
                cell.SetCellValue(cellVal);
            }
        }

        /// <summary>
        /// 根据单元格类型读取值
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns></returns>
        private object GetCellValue(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            if (cell.CellType == CellType.Boolean)
            {
                return cell.BooleanCellValue;
            }
            else if (cell.CellType == CellType.Numeric && HSSFDateUtil.IsCellDateFormatted(cell))//日期类型
            {
                //NPOI中数字和日期都是NUMERIC类型的，这里对其进行判断是否是日期类型
                return cell.DateCellValue;
            }
            else if ((cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric) || cell.CellType == CellType.Numeric)
            {
                //检测是否是百分比 是的话，手动除以100，因为程序里通常存的是50，表示50%
                string formatCode = _df.GetFormat(cell.CellStyle.DataFormat);
                if (formatCode.EndsWith("%"))
                {
                    return cell.NumericCellValue * 100;
                }
                else
                {
                    return cell.NumericCellValue;
                }
            }
            else
            {
                return cell.ToString();
            }
        }

        /// <summary>
        /// 应用规则
        /// </summary>
        private void ApplyRule()
        {
            //单独存放一个工作簿存放所有的数据
            var sheetName = $"ref_rule";
            var sheetRef = _workbook.CreateSheet(sheetName);//名为ref的工作表  

            //循环 i 作为数据存储的列
            // 第一组数据 存储在第一列
            // 第二组数据 存储在第二列
            // 以此类推
            for (int i = 0; i < _rules.Count; i++)
            {
                var rule = _rules[i];

                var options = rule.Source;

                if (options.Count == 0)
                {
                    continue;
                }

                //在当前列 保存所有数据, 所有数据是存储在行上
                for (var j = 0; j < options.Count; j++)
                {
                    var row = sheetRef.GetRow(j) ?? sheetRef.CreateRow(j);
                    row.CreateCell(i).SetCellValue(options[j]);
                }

                //在Excel工作簿中增加此数据范围
                IName range = _workbook.CreateName();//创建一个命名公式  

                //获取当前列的字符串列信息，如 1=>A ， 2=>B
                var colChar = PowerExcelConfigCell.ToColumnChar(i + 1);

                //公式范围格式应为：A1-A50  B1:B25
                range.RefersToFormula = $"{sheetName}!${colChar}$1:${colChar}${options.Count}";//公式内容，就是上面的区域  
                range.NameName = $"rule_{i}";//公式名称，可以在"公式"-->"名称管理器"中看到

                //规则也保存该名称，留着后面使用
                rule.RuleName = range.NameName;

                //if (rule.RowField != null && _dtList.Rows.Count > 0)
                //应用到行级字段
                if (rule.RowField != null)
                {
                    foreach (var field in rule.RowField)
                    {
                        SetField2Select(range.NameName, field);
                    }
                }
                //应用到填充字段
                if (rule.FillField != null)
                {
                    foreach (var field in rule.FillField)
                    {
                        SetField2Select(range.NameName, field);
                    }
                }
            }

            var index = _workbook.GetSheetIndex(sheetRef);
            _workbook.SetSheetHidden(index, _config.Prop.Debug ? false : true);
        }

        /// <summary>
        /// 设置字段为下拉框
        /// </summary>
        /// <param name="ruleName">规则名称</param>
        /// <param name="fieldName">字段名称</param>
        private void SetField2Select(string ruleName, string fieldName)
        {
            //查找字符索引
            var field = _config.Row.Where(t => t.Field == fieldName).FirstOrDefault();
            if (field == null)
            {
                return;
            }

            CellRangeAddressList regions = new CellRangeAddressList(_config.Prop.StartRow - 1, 65535, field.ColumnIndex, field.ColumnIndex);//约束范围：B1到B65535  
            XSSFDataValidationHelper helper = new XSSFDataValidationHelper((XSSFSheet)_sheet);//获得一个数据验证Helper  
            IDataValidation validation = helper.CreateValidation(helper.CreateFormulaListConstraint(ruleName), regions);//创建一个特定约束范围内的公式列表约束（即第一节里说的"自定义"方式）  
            validation.EmptyCellAllowed = true;
            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");//不符合约束时的提示  
            validation.ShowErrorBox = true;//显示上面提示 = True  
            _sheet.AddValidationData(validation);//添加进去  
        }


        #endregion

        #region API - 写入

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="filePath">xml路径</param>
        public void LoadXmlConfig(string filePath)
        {
            var xFileName = filePath.Replace(Path.GetFileName(filePath), "x_" + Path.GetFileName(filePath));
            if (File.Exists(xFileName))
            {
                filePath = xFileName;
            }
            if (File.Exists(filePath) == false)
            {
                throw new Exception("xml配置文件不存在！");
            }
            var content = File.ReadAllText(filePath);
            _config = XmlHelper.XmlDeserialize<PowerExcelConfig>(content);
            _config.ResolveConfig();
        }

        /// <summary>
        /// 渲染数据
        /// </summary>
        /// <param name="dt"></param>
        public void RenderFillData(DataTable dt)
        {
            this._dtFill = dt;
        }

        /// <summary>
        /// 渲染列表
        /// </summary>
        /// <param name="dt">列表数据</param>
        public void RenderList(DataTable dt)
        {
            this._dtList = dt;
        }

        /// <summary>
        /// 通过配置sql加载数据源
        /// </summary>
        /// <param name="param">参数</param>
        public void AutoRenderFillData(object param)
        {
            //_dtFill = CPQuery.From(_config.Prop.FillSql, param).ToDataTable();
        }

        /// <summary>
        /// 通过配置sql加载数据源
        /// </summary>
        /// <param name="param">参数</param>
        public void AutoRenderList(object param)
        {
            //_dtList = CPQuery.From(_config.Prop.ListSql, param).ToDataTable();
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public void SaveFile(string path)
        {
            try
            {
                OnBeforeSave();
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (FileStream fileTo = new FileStream(path, FileMode.OpenOrCreate))
                {
                    _workbook.Write(fileTo);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 转成字节流
        /// </summary>
        /// <returns></returns>
        public byte[] Save2Bytes()
        {
            OnBeforeSave();
            using (MemoryStream ms = new MemoryStream())
            {
                _workbook.Write(ms);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// 应用下拉框数据源到某些字段
        /// </summary>
        /// <param name="source">字符串数据源</param>
        /// <param name="fields">字段，可以输入多个</param>
        public void ApplyDropdownField(List<string> source, params string[] fields)
        {
            _rules.Add(new PowerExcelRule()
            {
                Source = source,
                RowField = fields
            });
        }
        #endregion

        #region API - 读取


        /// <summary>
        /// 校验单元格是否通过验证
        /// </summary>
        /// <param name="rowIndex">行号</param>
        /// <param name="field">字段列信息</param>
        /// <param name="cellVal"></param>
        private void ValidateCellValue(int rowIndex, PowerExcelConfigCell field, object cellVal)
        {
            var attr = field.Attribute;
            var isEmptyCell = cellVal == null || string.IsNullOrEmpty(cellVal.ToString());
            //没有配置校验规则 就放弃校验
            if (attr != null)
            {
                //校验不能为空
                if (attr.Required && isEmptyCell == true)
                {
                    _errors.Add(new ErrorDTO($"{field.Title}  ", $"{field.Title}为必填项") { ColName = field.Col, RowIndex = rowIndex + 1 });
                    return;
                }
                //校验邮箱
                if (attr.Validate == nameof(ValidateRule.mail) && isEmptyCell == false)
                {
                    Regex _mail = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
                    if (_mail.IsMatch(cellVal.ToString()) == false)
                    {
                        _errors.Add(new ErrorDTO($"{field.Title} ", $"邮箱:{cellVal} 格式不正确") { ColName = field.Col, RowIndex = rowIndex + 1 });
                    }
                }
            }
            //空数据就没有必要做其他类型的校验
            if (isEmptyCell)
            {
                return;
            }
            //存在数据在做校验 金额列校验最大值和最小值 
            if (field.CellType == "number")
            {
                double val;
                if (cellVal is double)
                {
                    val = (double)cellVal;
                }
                else if (double.TryParse(cellVal.ToString(), out val) == false)
                {
                    _errors.Add(new ErrorDTO($"{field.Title} ", $"无效数值{cellVal}") { ColName = field.Col, RowIndex = rowIndex + 1 });
                }
                if (attr != null)
                {
                    if (val < attr.Min)
                    {
                        _errors.Add(new ErrorDTO($"{field.Title} 超过最小值", $"{val} 小于 {attr.Min}") { ColName = field.Col, RowIndex = rowIndex + 1 });
                    }
                    else if (val > attr.Max)
                    {
                        _errors.Add(new ErrorDTO($"{field.Title} 超过最大值", $"{val} 大于 {attr.Max}") { ColName = field.Col, RowIndex = rowIndex + 1 });
                    }
                }
            }
            else if (field.CellType == "text")
            {

            }
            else if (field.CellType == "date")
            {
                //单元格如果是日期格式，在非空前提下，必须校验时间格式
                if (isEmptyCell == false)
                {
                    DateTime val;
                    if (cellVal is DateTime)
                    {
                        val = (DateTime)cellVal;
                    }
                    else if (DateTime.TryParse(cellVal.ToString(), out val) == false)
                    {
                        _errors.Add(new ErrorDTO($"{field.Title} ", $"时间值，{cellVal} 格式不正确") { ColName = field.Col, RowIndex = rowIndex + 1 });
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据，不读取缓存
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetListWithoutCache()
        {
            //获取总行号
            var rowEnd = _sheet.LastRowNum;

            var list = new List<Dictionary<string, object>>();

            //循环列表数组
            // 减1 是因为生成的Excel删掉了demo行
            for (var i = _config.Prop.StartRow - 1; i <= rowEnd; i++)
            {
                var row = _sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }
                var dic = new Dictionary<string, object>();
                //字段信息即可找到存储的内容
                _config.Row.ForEach(field =>
                {
                    var cell = row.GetCell(field.ColumnIndex);
                    if (cell == null)
                    {
                        return;
                    }
                    var cellVal = GetCellValue(cell);
                    dic[field.Field] = cellVal;
                    //校验
                    ValidateCellValue(i, field, cellVal);
                });

                list.Add(dic);
            }

            //获取缓存数据
            return list;
        }

        /// <summary>
        /// 获取数据，读取缓存
        /// </summary>
        /// <param name="validate">自定义验证</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetList(Action<int, Dictionary<string, object>> validate = null)
        {
            //获取总行号
            var rowEnd = _sheet.LastRowNum;

            var list = new List<Dictionary<string, object>>();

            var keyField = string.IsNullOrEmpty(_config.Prop.ListKey) ? null : _config.Row.FirstOrDefault(t => t.Field == _config.Prop.ListKey);

            //循环列表数组
            // 减1 是因为生成的Excel删掉了demo行
            for (var i = _config.Prop.StartRow - 1; i <= rowEnd; i++)
            {
                var row = _sheet.GetRow(i);
                if (row == null)
                {
                    break;
                }

                //定义当前数据行
                var dic = new Dictionary<string, object>();

                //判断数据是否是有效行
                if (keyField != null)
                {
                    //获取唯一键单元格
                    var cell = row.GetCell(keyField.ColumnIndex);
                    if (cell == null)
                    {
                        continue;
                    }
                    //获取唯一键数据
                    var cellVal = GetCellValue(cell);
                    if (cellVal == null || string.IsNullOrEmpty(cellVal.ToString()))
                    {
                        continue;
                    }
                }

                //字段信息即可找到存储的内容
                //读取默认header 保持结构完整
                _config.Row.ForEach(field =>
                {
                    var cell = row.GetCell(field.ColumnIndex);
                    object cellVal = string.Empty;
                    if (cell != null)
                    {
                        cellVal = GetCellValue(cell);
                    }
                    dic[field.Field] = cellVal;
                    if (_sheet.IsColumnHidden(field.ColumnIndex) == false)
                    {
                        //使用Excel自带验证规则
                        ValidateCellValue(i, field, cellVal);
                    }
                });

                //自定义校验
                validate?.Invoke(i, dic);

                list.Add(dic);
            }

            //获取缓存数据
            return GetCacheData(_listSheet, list);
        }



        /// <summary>
        /// 获取数据域信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetFillData()
        {
            var dic = new Dictionary<string, object>();
            foreach (var field in _config.Fill)
            {
                var thisCell = _sheet.GetRow(field.RowIndex).GetCell(field.ColumnIndex);
                if (thisCell == null)
                {
                    continue;
                }
                dic[field.Field] = GetCellValue(thisCell);
            }

            var mergeDic = GetCacheData(_fillSheet, new List<Dictionary<string, object>>() { dic });
            return mergeDic.FirstOrDefault();
        }
        #endregion

        #region API - 错误信息

        /// <summary>
        /// 读取错误信息
        /// </summary>
        /// <returns></returns>
        public List<ErrorDTO> GetErrors()
        {
            return _errors.OrderBy(t => t.RowIndex).ThenBy(t => t.ColName).ToList();
        }


        /// <summary>
        /// 增加错误处理
        /// </summary>
        /// <param name="rowIndex">错误所在行号，该参数直接读取Excel的数据里的行号，从0开始</param>
        /// <param name="colName">错误列，字段名即可</param>
        /// <param name="title">错误标题</param>
        /// <param name="reason">错误详细</param>
        public void AddErrors(int rowIndex, string colName, string title, string reason)
        {
            var field = _config.Row.FirstOrDefault(t => t.Field == colName);
            if (field != null)
            {
                //数据的行号，要转换成Excel的行号
                //转换算法： 数据起始行 + 数据索引 - 1 + 1
                //减去1的原因是因为，存在demo行，导出的时候已经删掉了
                //加上1的原因是因为，数据索引行从0开始，修改为从1计数
                //这里没有省略，是为了以后看清楚为什么要这么写，不是多余的
                rowIndex = _config.Prop.StartRow - 1 + rowIndex + 1;
                _errors.Add(new ErrorDTO(title, reason) { RowIndex = rowIndex, ColName = field.GetColumnChar() });
            }
        }

        #endregion

        #region API - 校验

        /// <summary>
        /// 校验模板正确性
        /// 注意：先加载xml
        /// </summary>
        public void Validate()
        {
            //需要先加载xml配置
            if (_config == null)
            {
                throw new Exception("请先加载xml配置，再调用验证！");
            }

            //是否包含2个常用的模板
            //var sheetFill = _workbook.GetSheet(_fillSheet);
            var sheetList = _workbook.GetSheet(_listSheet);

            if (sheetList == null)
            {
                throw new Exception("模板不正确！");
            }

            //两者均不包含，说明就有问题了
            //if (sheetFill == null && sheetList == null)
            //{
            //    throw new Exception("模板不正确！");
            //}

            //存在sheet，就获取列名，与配置项比较
            //if (sheetFill != null)
            //{
            //    //var row = sheetFill.GetRow(0);
            //    ////获取所有列名
            //    //var allName = row.Cells.Where(t => t != null).Select(t => t.ToString()).ToList();
            //    ////更新填充列
            //    //_config.Fill.ForEach(t =>
            //    //{
            //    //    //填充域不校验公式字段
            //    //    //因为有可能是一个无意义的列
            //    //    if (string.IsNullOrEmpty(t.Formula) && allName.Contains(t.Field) == false)
            //    //    {
            //    //        throw new BusinessLogicException("模板不正确，请先导出！");
            //    //    }
            //    //});
            //}
            if (sheetList != null)
            {
                var row = sheetList.GetRow(1);
                //获取所有列名
                var allName = row.Cells.Where(t => t != null).Select(t => t.ToString()).ToList();
                //更新填充列
                _config.Row.ForEach(t =>
                {
                    if (allName.Contains(t.Title) == false)
                    {
                        throw new Exception("模板不正确，请先导出！");
                    }
                });
            }
        }

        #endregion
    }
}
