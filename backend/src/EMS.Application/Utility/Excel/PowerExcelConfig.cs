using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace EMS.Application.Utility.Excel
{
    /// <summary>
    /// xml配置根节点对象
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot(ElementName = "config", Namespace = "", IsNullable = false)]
    public class PowerExcelConfig
    {
        /// <summary>
        /// 整体属性配置
        /// </summary>
        [XmlElement("prop")]
        public PowerExcelConfigProp Prop { get; set; }

        /// <summary>
        /// 填充行列配置
        /// </summary>
        [XmlArrayItem("cell")]
        [XmlArray("fill")]
        public List<PowerExcelConfigCell> Fill { get; set; }

        /// <summary>
        /// 行级配置
        /// </summary>
        [XmlArrayItem("cell")]
        [XmlArray("row")]
        public List<PowerExcelConfigCell> Row { get; set; }

        /// <summary>
        /// 解析配置
        /// </summary>
        public void ResolveConfig()
        {
            // 重置填充单元格的行号
            Fill.ForEach(t =>
            {
                t.RowIndex--;
                t.ColumnIndex = PowerExcelConfigCell.ToIndex(t.Col);
            });

            //设置整体属性
            Prop.StartColumnIndex = PowerExcelConfigCell.ToIndex(Prop.StartCol);
            Prop.DemoRow--;
            Prop.StartRow = Prop.DemoRow + 1;

            //设置循环列的 列索引号
            var i = 0;
            Row.ForEach(field =>
            {
                field.ColumnIndex = Prop.StartColumnIndex + i++;
                field.Col = field.GetColumnChar();
            });


            //设置列的公式信息 将$Field转换成 D{i}
            Row.Where(t => string.IsNullOrEmpty(t.Formula) == false).ToList().ForEach(field =>
            {
                field.Formula = Regex.Replace(field.Formula, @"\$(?<name>\w+)", (match) =>
                {
                    var name = match.Groups["name"]?.Value;
                    if (string.IsNullOrEmpty(name))
                    {
                        return match.Value;
                    }

                    //查找源字段 得到公式信息
                    var index = Row.FirstOrDefault(t => t.Field == name);
                    if (index == null)
                    {
                        throw new Exception($"行字段中未找到{name}");
                    }
                    return index.GetColumnChar() + "{i}";
                });
            });
        }
    }

    /// <summary>
    /// 整体属性配置
    /// </summary>
    [Serializable]
    public class PowerExcelConfigProp
    {
        /// <summary>
        /// 是否调试
        /// </summary>
        [XmlElement("debug")]
        public bool Debug { get; set; }

        /// <summary>
        /// 显示序号
        /// </summary>
        [XmlElement("showIndex")]
        public bool ShowIndex { get; set; }

        /// <summary>
        /// 模板行
        /// </summary>
        [XmlElement("demoRow")]
        public int DemoRow { get; set; }

        /// <summary>
        /// 列表数据起始行
        /// </summary>
        [XmlElement("startRow")]
        public int StartRow { get; set; }

        /// <summary>
        /// 列表数据起始行列
        /// </summary>
        [XmlElement("startCol")]
        public string StartCol { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int StartColumnIndex { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [XmlElement("password")]
        public string Password { get; set; }

        /// <summary>
        /// 填充数据查询SQL
        /// </summary>
        [XmlElement("fillSql")]
        public string FillSql { get; set; }

        /// <summary>
        /// 列表数据查询SQL
        /// </summary>
        [XmlElement("listSql")]
        public string ListSql { get; set; }

        /// <summary>
        /// 列表数据行唯一键 如果数据该行为空，则不触发该校验
        /// </summary>
        [XmlElement("listKey")]
        public string ListKey { get; set; }
    }

    /// <summary>
    /// 填充配置
    /// </summary>
    [Serializable]
    public class PowerExcelConfigFill
    {
        /// <summary>
        /// 填充单元格配置
        /// </summary>
        [XmlElement("cell")]
        public List<PowerExcelConfigCell> Cell { get; set; }
    }

    /// <summary>
    /// 单元格配置
    /// </summary>
    [Serializable]
    public class PowerExcelConfigCell
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        [XmlAttribute("field")]
        public string Field { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [XmlAttribute("celltype")]
        public string CellType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CellType GetCellType()
        {
            if (CellType == "text")
            {
                return NPOI.SS.UserModel.CellType.String;
            }
            switch (CellType)
            {
                case "empty": return NPOI.SS.UserModel.CellType.Unknown;
                case "number": return NPOI.SS.UserModel.CellType.Numeric;
                case "date": return NPOI.SS.UserModel.CellType.Numeric;
                case "text": return NPOI.SS.UserModel.CellType.String;
            }
            return NPOI.SS.UserModel.CellType.Unknown;
        }

        /// <summary>
        /// 行索引
        /// </summary>
        [XmlAttribute("row")]
        public int RowIndex { get; set; }

        /// <summary>
        /// 列索引，或者列号字母
        /// </summary>
        [XmlAttribute("col")]
        public string Col { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 公式信息
        /// </summary>
        [XmlAttribute("formula")]
        public string Formula { get; set; }
        /// <summary>
        /// 属性信息
        /// </summary>

        [XmlElement("attribute")]
        public PowerExcelConfigCellAttribute Attribute { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [XmlAttribute("isHidden")]
        public string IsHidden { get; set; }

        /// <summary>
        /// 获取字符串列名信息
        /// </summary>
        /// <returns></returns>
        public string GetColumnChar()
        {
            return ToColumnChar(ColumnIndex + 1);
        }

        /// <summary>
        /// 字符列名转数字
        /// </summary>
        /// <param name="columnName">字符串列名</param>
        /// <returns></returns>
        public static int ToIndex(string columnName)
        {
            //列名为数字 直接返回
            if (Regex.IsMatch(columnName, @"\d+"))
            {
                return Convert.ToInt32(columnName);
            }

            //检测是否是纯字母列名
            if (Regex.IsMatch(columnName.ToUpper(), @"[A-Z]+") == false) { throw new Exception("invalid parameter"); }

            //转换成数值
            int index = 0;
            char[] chars = columnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index - 1;
        }

        /// <summary>
        /// 将数字列名 转换为 字符列名
        /// </summary>
        /// <param name="columnIndex">数字列信息</param>
        /// <returns></returns>
        public static string ToColumnChar(int columnIndex)
        {
            var number = columnIndex;
            if (1 <= number && 36 >= number)
            {
                int num = number + 64;
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)num };
                return asciiEncoding.GetString(btNumber);
            }
            return "数字不在转换范围内";
        }
    }

    /// <summary>
    /// 单元格属性配置
    /// </summary>
    [Serializable]
    public class PowerExcelConfigCellAttribute
    {
        /// <summary>
        /// 最小值
        /// </summary>
        [XmlAttribute("min")]
        public double Min { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        [XmlAttribute("max")]
        public double Max { get; set; }
        /// <summary>
        /// 字符串长度
        /// </summary>

        [XmlAttribute("maxlength")]
        public int Length { get; set; }

        /// <summary>
        /// 非空验证
        /// </summary>
        [XmlAttribute("required")]
        public bool Required { get; set; }

        /// <summary>
        /// 非空
        /// </summary>
        [XmlAttribute("validate")]
        public string Validate { get; set; }
    }

    /// <summary>
    /// 验证规则
    /// </summary>
    public enum ValidateRule
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        mail = 3,
    }

    /// <summary>
    /// 下拉框数据源规则
    /// </summary>
    public class PowerExcelRule
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public List<string> Source { get; set; }

        /// <summary>
        /// 要应用的填充单元格字段信息
        /// </summary>
        public string[] FillField { get; set; }

        /// <summary>
        /// 要应用的填充单元格字段信息
        /// </summary>
        public string[] RowField { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string RuleName { get; set; }
    }
}
