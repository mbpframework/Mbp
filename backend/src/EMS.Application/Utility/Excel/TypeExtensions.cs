using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Utility.Excel
{
    /// <summary>
    /// 提供类型扩展方法
    /// </summary>
    public static class TypeExtensions
    {
        #region 类型转换

        /// <summary>
        /// 安全类型转换，转换失败返回null，不抛出异常
        /// </summary>
        /// <typeparam name="T">转换类型</typeparam>
        /// <param name="obj">待转换的对象</param>
        /// <returns>null值将转换成类型默认值</returns>
        public static T ConvertType<T>(object obj)
        {
            var result = ConvertType(typeof(T), obj);
            return result == null ? default(T) : (T)result;
        }

        /// <summary>
        /// 安全类型转换，转换失败返回null，不抛出异常
        /// </summary>
        /// <param name="type">转换类型</param>
        /// <param name="obj">待转换的对象</param>
        /// <returns>null值不做处理，返回null</returns>
        public static object ConvertType(Type type, object obj)
        {
            //代码注释
            //代码注释
            //代码注释
            //代码注释

            if (obj == null || obj == DBNull.Value)
            {
                return null;
            }

            if (type.IsGenericType && type.Name.Contains("Nullable`"))
            {
                Type realType = type.GetGenericArguments()[0];
                return ConvertType(realType, obj);
            }

            if (obj is string)
            {
                if (type == typeof(Guid))
                {
                    return new Guid((string)obj);
                }

                if (type == typeof(DateTime))
                {
                    return DateTime.Parse((string)obj);
                }
            }

            return Convert.ChangeType(obj, type) ?? ConvertType(type, obj.ToString());

        }

        #endregion

        /// <summary>
        /// 字符串转decimal
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static decimal AsDecimal(this string str, decimal defaultVal)
        {
            decimal d;
            if (Decimal.TryParse(str, out d))
            {
                return d;
            }
            return defaultVal;
        }

        /// <summary>
        /// 字符串转decimal
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static decimal AsDecimal(this string str) => str.AsDecimal(default(decimal));


        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static double AsDouble(this string str, double defaultVal)
        {
            double d;
            if (Double.TryParse(str, out d))
            {
                return d;
            }
            return defaultVal;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double AsDouble(this string str) => str.AsDouble(default(double));

        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static int AsInt(this string str, int defaultVal)
        {
            int d;
            if (int.TryParse(str, out d))
            {
                return d;
            }
            return defaultVal;
        }

        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int AsInt(this string str) => str.AsInt(default(int));

        /// <summary>
        /// 将字符串转换为时间
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static DateTime AsDateTime(this string str)
        {
            DateTime time;
            if (DateTime.TryParse(str, out time))
            {
                return time;
            }
            return DateTime.MinValue;
        }


        /// <summary>
        /// 将字符串转换为GUID
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static Guid AsGUID(this string str)
        {
            Guid valGuid;
            if (Guid.TryParse(str, out valGuid))
            {
                return valGuid;
            }
            return Guid.Empty;
        }
    }
}
