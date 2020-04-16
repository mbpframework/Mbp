using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EMS.Application.Utility.Excel
{
    /// <summary>
    /// XML辅助类
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// XML枚举类型
        /// </summary>
        public enum XmlType
        {
            /// <summary>
            /// 文件
            /// </summary>
            File,

            /// <summary>
            /// 字符串
            /// </summary>
            String
        };

        /// <summary>
        /// 创建XML文档
        /// </summary>
        /// <param name="name">根节点名称</param>
        /// <param name="type">根节点的一个属性值</param>
        /// <returns></returns>
        /// document = XmlOperate.CreateXmlDocument("sex", "sexy");
        /// document.Save("c:/bookstore.xml");        
        public static XmlDocument CreateXmlDocument(string name, string type)
        {
            var doc = new XmlDocument();
            doc.LoadXml("<" + name + "/>");
            var rootEle = doc.DocumentElement;
            if (rootEle != null)
            {
                rootEle.SetAttribute("type", type);
            }
            return doc;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Read(path, "/Node", "")
         * XmlHelper.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
         ************************************************/
        public static string Read(string path, string node, string attribute)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode xn = doc.SelectSingleNode(node);
            var value = string.Empty;
            if (xn != null)
            {
                if (xn.Attributes != null)
                {
                    value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
                }
            }
            return value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "Element", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Element", "Attribute", "Value")
         * XmlHelper.Insert(path, "/Node", "", "Attribute", "Value")
         ************************************************/
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            //创建xml对象
            XmlDocument doc = new XmlDocument();
            //加载文档对象
            doc.Load(path);
            //查询文档节点
            XmlNode xn = doc.SelectSingleNode(node);
            //节点是否存在
            if (element.Equals(""))
            {
                //树形是否存在
                if (attribute.Equals("") == false)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe != null)
                    {
                        //设置树形
                        xe.SetAttribute(attribute, value);
                    }
                }
            }
            else
            {
                //创建节点
                XmlElement xe = doc.CreateElement(element);
                if (attribute.Equals(""))
                {
                    //设置节点值
                    xe.InnerText = value;
                }

                else
                {
                    //设置节点树形
                    xe.SetAttribute(attribute, value);
                }
                if (xn != null)
                {
                    //添加孩子节点
                    xn.AppendChild(xe);
                }
            }
            //保存对象
            doc.Save(path);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Insert(path, "/Node", "", "Value")
         * XmlHelper.Insert(path, "/Node", "Attribute", "Value")
         ************************************************/
        public static void Update(string path, string node, string attribute, string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode xn = doc.SelectSingleNode(node);
            XmlElement xe = (XmlElement)xn;
            if (attribute.Equals(""))
            {
                if (xe != null)
                {
                    xe.InnerText = value;
                }
            }
            else
            {
                if (xe != null)
                {
                    xe.SetAttribute(attribute, value);
                }
            }
            doc.Save(path);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <returns></returns>
        /**************************************************
         * 使用示列:
         * XmlHelper.Delete(path, "/Node", "")
         * XmlHelper.Delete(path, "/Node", "Attribute")
         ************************************************/
        public static void Delete(string path, string node, string attribute)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNode xn = doc.SelectSingleNode(node);
            XmlElement xe = (XmlElement)xn;
            if (attribute.Equals(""))
            {
                if (xn != null)
                {
                    xn.ParentNode.RemoveChild(xn);
                }
            }
            else
            {
                if (xe != null)
                {
                    xe.RemoveAttribute(attribute);
                }
            }
            doc.Save(path);
        }

        #region 读取XML资源到DataSet中
        /// <summary>
        /// 读取XML资源到DataSet中
        /// </summary>
        /// <param name="source">XML资源，文件为路径，否则为XML字符串</param>
        /// <param name="xmlType">XML资源类型</param>
        /// <returns>DataSet</returns>
        public static DataSet GetDataSet(string source, XmlType xmlType)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds;
        }

        #endregion

        #region 操作xml文件中指定节点的数据
        /// <summary>
        /// 获得xml文件中指定节点的节点数据
        /// </summary>
        /// <param name="path">xml路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static string GetNodeInfoByNodeName(string path, string nodeName)
        {
            string xmlString = string.Empty;
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            var root = xml.DocumentElement;
            if (root != null)
            {
                var node = root.SelectSingleNode("//" + nodeName);
                if (node != null)
                {
                    xmlString = node.InnerText;
                }
            }
            return xmlString;
        }
        #endregion

        #region 获取一个字符串xml文档中的ds
        /// <summary>
        /// 获取一个字符串xml文档中的ds
        /// </summary>
        /// <param name="xmlString">含有xml信息的字符串</param>
        ///<param name = "ds"> 存储位置 </param >
        public static void GetXmlValueDs(string xmlString, ref DataSet ds)
        {
            var xd = new XmlDocument();
            xd.LoadXml(xmlString);
            XmlNodeReader xnr = new XmlNodeReader(xd);
            ds.ReadXml(xnr);
            xnr.Close();
        }
        #endregion

        #region 读取XML资源到DataTable中
        /// <summary>
        /// 读取XML资源到DataTable中
        /// </summary>
        /// <param name="source">XML资源，文件为路径，否则为XML字符串</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="tableName">表名称</param>
        /// <returns>DataTable</returns>
        public static DataTable GetTable(string source, XmlType xmlType, string tableName)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds.Tables[tableName];
        }
        #endregion

        #region 读取XML资源中指定的DataTable的指定行指定列的值
        /// <summary>
        /// 读取XML资源中指定的DataTable的指定行指定列的值
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colName">列名</param>
        /// <returns>值，不存在时返回Null</returns>
        public static object GetTableCell(string source, XmlType xmlType, string tableName, int rowIndex, string colName)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds.Tables[tableName].Rows[rowIndex][colName];
        }
        #endregion

        #region 读取XML资源中指定的DataTable的指定行指定列的值
        /// <summary>
        /// 读取XML资源中指定的DataTable的指定行指定列的值
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colIndex">列号</param>
        /// <returns>值，不存在时返回Null</returns>
        public static object GetTableCell(string source, XmlType xmlType, string tableName, int rowIndex, int colIndex)
        {
            DataSet ds = new DataSet();
            if (xmlType == XmlType.File)
            {
                ds.ReadXml(source);
            }
            else
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xd);
                ds.ReadXml(xnr);
            }
            return ds.Tables[tableName].Rows[rowIndex][colIndex];
        }
        #endregion

        #region 将DataTable写入XML文件中
        /// <summary>
        /// 将DataTable写入XML文件中
        /// </summary>
        /// <param name="dt">含有数据的DataTable</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveTableToFile(DataTable dt, string filePath)
        {
            DataSet ds = new DataSet("Config");
            ds.Tables.Add(dt.Copy());
            ds.WriteXml(filePath);
        }
        #endregion

        #region 将DataTable以指定的根结点名称写入文件
        /// <summary>
        /// 将DataTable以指定的根结点名称写入文件
        /// </summary>
        /// <param name="dt">含有数据的DataTable</param>
        /// <param name="rootName">根结点名称</param>
        /// <param name="filePath">文件路径</param>
        public static void SaveTableToFile(DataTable dt, string rootName, string filePath)
        {
            DataSet ds = new DataSet(rootName);
            ds.Tables.Add(dt.Copy());
            ds.WriteXml(filePath);
        }
        #endregion

        #region 使用DataSet方式更新XML文件节点
        /// <summary>
        /// 使用DataSet方式更新XML文件节点
        /// </summary>
        /// <param name="filePath">XML文件路径</param>
        /// <param name="tableName">表名称</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colName">列名</param>
        /// <param name="content">更新值</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateTableCell(string filePath, string tableName, int rowIndex, string colName, string content)
        {
            bool flag = false;
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);
            DataTable dt = ds.Tables[tableName];

            if (dt.Rows[rowIndex][colName] != null)
            {
                dt.Rows[rowIndex][colName] = content;
                ds.WriteXml(filePath);
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 使用DataSet方式更新XML文件节点
        /// <summary>
        /// 使用DataSet方式更新XML文件节点
        /// </summary>
        /// <param name="filePath">XML文件路径</param>
        /// <param name="tableName">表名称</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colIndex">列号</param>
        /// <param name="content">更新值</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateTableCell(string filePath, string tableName, int rowIndex, int colIndex, string content)
        {
            bool flag = false;
            DataSet ds = new DataSet();
            ds.ReadXml(filePath);
            DataTable dt = ds.Tables[tableName];

            if (dt.Rows[rowIndex][colIndex] != null)
            {
                dt.Rows[rowIndex][colIndex] = content;
                ds.WriteXml(filePath);
                flag = true;
            }
            return flag;
        }
        #endregion

        #region 读取XML资源中的指定节点内容
        /// <summary>
        /// 读取XML资源中的指定节点内容
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点内容</returns>
        public static object GetNodeValue(string source, XmlType xmlType, string nodeName)
        {
            XmlDocument xd = new XmlDocument();
            if (xmlType == XmlType.File)
            {
                xd.Load(source);
            }
            else
            {
                xd.LoadXml(source);
            }
            XmlElement xe = xd.DocumentElement;
            XmlNode xn = xe?.SelectSingleNode("//" + nodeName);
            if (xn != null)
            {
                return xn.InnerText;
            }
            return "暂无数据";
        }

        /// <summary>
        /// 读取XML资源中的指定节点内容
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点内容</returns>
        public static object GetNodeValue(string source, string nodeName)
        {
            if (source == null || nodeName == null || string.IsNullOrEmpty(source) || string.IsNullOrEmpty(nodeName) || source.Length < nodeName.Length * 2)
            {
                return null;
            }
            int start = source.IndexOf("<" + nodeName + ">", StringComparison.Ordinal) + nodeName.Length + 2;
            int end = source.IndexOf("</" + nodeName + ">", StringComparison.Ordinal);
            if (start == -1 || end == -1)
            {
                return null;
            }
            if (start >= end)
            {
                return null;
            }
            return source.Substring(start, end - start);
        }
        #endregion

        #region 更新XML文件中的指定节点内容
        /// <summary>
        /// 更新XML文件中的指定节点内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">更新内容</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateNode(string filePath, string nodeName, string nodeValue)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load(filePath);
            XmlElement xe = xd.DocumentElement;
            XmlNode xn = xe?.SelectSingleNode("//" + nodeName);
            if (xn == null) return false;
            xn.InnerText = nodeValue;
            return true;
        }
        #endregion


        /// <summary>
        /// 读取xml文件，并将文件序列化为类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        /// 
        public static T ReadXML<T>(string path)
        {
            //XmlSerializer reader = new XmlSerializer(typeof(JJDB));
            XmlSerializer reader = new XmlSerializer(typeof(string));
            StreamReader file = new StreamReader(@path);
            return (T)reader.Deserialize(file);
        }

        /// <summary>
        /// 将对象写入XML文件
        /// </summary>
        /// <typeparam name="T">C#对象名</typeparam>
        /// <param name="item">对象实例</param>
        /// <param name="path">路径</param>
        /// <param name="jjdbh">标号</param>
        /// <param name="ends">结束符号（整个xml的路径类似如下：C:xmltest201111send.xml，其中path=C:xmltest,jjdbh=201111,ends=send）</param>
        /// <returns></returns>
        public static string WriteXML<T>(T item, string path, string jjdbh, string ends)
        {
            if (string.IsNullOrEmpty(ends))
            {
                //默认为发送
                //ends = "send";
                ends = "";
            }
            //序列xml化对象
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            //创建xml文件名称
            object[] obj = { path, "", jjdbh, ends, ".xml" };
            //合并字符串
            string xmlPath = string.Concat(obj);
            while (true)
            {
                //用filestream方式创建文件不会出现“文件正在占用中，用File.create”则不行
                var fs = File.Create(xmlPath);
                fs.Close();
                //创建字节流
                TextWriter writer = new StreamWriter(xmlPath, false, Encoding.UTF8);
                XmlSerializerNamespaces xml = new XmlSerializerNamespaces();
                //xml添加
                xml.Add(string.Empty, string.Empty);
                //序列化对象
                serializer.Serialize(writer, item, xml);
                //清空缓冲区
                writer.Flush();
                //关闭
                writer.Close();
                break;
            }
            //序列化xml对象
            return SerializeToXmlStr<T>(item, true);
        }

        /// <summary>
        /// 静态扩展
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">true:省略XML声明;否则为false.默认false，即编写 XML 声明。</param>
        /// <returns></returns>
        public static string SerializeToXmlStr<T>(T obj, bool omitXmlDeclaration)
        {

            return XmlSerialize<T>(obj, omitXmlDeclaration);
        }

        #region XML序列化反序列化相关的静态方法
        /// <summary>
        /// 使用XmlSerializer序列化对象
        /// </summary>
        /// <typeparam name="T">需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">true:省略XML声明;否则为false.默认false，即编写 XML 声明。</param>
        /// <returns>序列化后的字符串</returns>
        public static string XmlSerialize<T>(T obj, bool omitXmlDeclaration)
        {

            /* This property only applies to XmlWriter instances that output text content to a stream; otherwise, this setting is ignored.
            可能很多朋友遇见过 不能转换成Xml不能反序列化成为UTF8XML声明的情况，就是这个原因。
            */
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitXmlDeclaration,
                Encoding = new UTF8Encoding(false)
            };
            MemoryStream stream = new MemoryStream();//var writer = new StringWriter();
            XmlWriter xmlwriter = XmlWriter.Create(stream/*writer*/, xmlSettings); //这里如果直接写成：Encoding = Encoding.UTF8 会在生成的xml中加入BOM(Byte-order Mark) 信息(Unicode 字节顺序标记) ， 所以new System.Text.UTF8Encoding(false)是最佳方式，省得再做替换的麻烦
            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty); //在XML序列化时去除默认命名空间xmlns:xsd和xmlns:xsi
            XmlSerializer ser = new XmlSerializer(typeof(T));
            ser.Serialize(xmlwriter, obj, xmlns);

            return Encoding.UTF8.GetString(stream.ToArray());//writer.ToString();
        }

        /// <summary>
        /// 使用XmlSerializer序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="omitXmlDeclaration">true:省略XML声明;否则为false.默认false，即编写 XML 声明。</param>
        /// <param name="removeDefaultNamespace">是否移除默认名称空间(如果对象定义时指定了:XmlRoot(Namespace = "http://www.xxx.com/xsd")则需要传false值进来)</param>
        /// <returns>序列化后的字符串</returns>
        public static void XmlSerialize<T>(string path, T obj, bool omitXmlDeclaration, bool removeDefaultNamespace)
        {
            XmlWriterSettings xmlSetings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitXmlDeclaration
            };
            using (XmlWriter xmlwriter = XmlWriter.Create(path, xmlSetings))
            {
                XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                if (removeDefaultNamespace)
                    xmlns.Add(string.Empty, string.Empty); //在XML序列化时去除默认命名空间xmlns:xsd和xmlns:xsi
                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.Serialize(xmlwriter, obj, xmlns);
            }
        }

        private static byte[] ShareReadFile(string filePath)
        {
            byte[] bytes;
            //避免"正由另一进程使用,因此该进程无法访问此文件"造成异常 共享锁 flieShare必须为ReadWrite，但是如果文件不存在的话，还是会出现异常，所以这里不能吃掉任何异常，但是需要考虑到这些问题
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                bytes = new byte[fs.Length];
                int numBytesToRead = (int)fs.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    int n = fs.Read(bytes, numBytesRead, numBytesToRead);
                    if (n == 0)
                        break;
                    numBytesRead += n;
                    numBytesToRead -= n;
                }
            }
            return bytes;
        }

        /// <summary>
        /// 从文件读取并反序列化为对象 （解决: 多线程或多进程下读写并发问题）
        /// </summary>
        /// <typeparam name="T">返回的对象类型</typeparam>
        /// <param name="path">文件地址</param>
        /// <returns></returns>
        public static T XmlFileDeserialize<T>(string path)
        {
            byte[] bytes = ShareReadFile(path);
            if (bytes.Length < 1)//当文件正在被写入数据时，可能读出为0
                for (int i = 0; i < 5; i++)
                { //5次机会
                    bytes = ShareReadFile(path); // 采用这样诡异的做法避免独占文件和文件正在被写入时读出来的数据为0字节的问题。
                    if (bytes.Length > 0) break;
                    System.Threading.Thread.Sleep(50); //悲观情况下总共最多消耗1/4秒，读取文件
                }
            XmlDocument doc = new XmlDocument();
            doc.Load(new MemoryStream(bytes));
            if (doc.DocumentElement != null)
                return (T)new XmlSerializer(typeof(T)).Deserialize(new XmlNodeReader(doc.DocumentElement));
            return default(T);
        }

        /// <summary>
        /// 使用XmlSerializer反序列化对象
        /// </summary>
        /// <param name="xmlOfObject">需要反序列化的xml字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T XmlDeserialize<T>(string xmlOfObject) where T : class
        {
            XmlReader xmlReader = XmlReader.Create(new StringReader(xmlOfObject), new XmlReaderSettings());
            return (T)new XmlSerializer(typeof(T)).Deserialize(xmlReader);
        }

        #endregion
    }
}
