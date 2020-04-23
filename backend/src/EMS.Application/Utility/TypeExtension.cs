using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Utility
{
    public static class TypeExtension
    {
        public static string ListToStrByComma<T>(this List<T> list)
        {
            return string.Join(",", list);
        }

        public static List<T> StrToListByComma<T>(this string str)
        {
            if (string.IsNullOrEmpty(str)) return new List<T>();

            List<T> list = new List<T>();

            foreach (var item in str.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                list.Add((T)Convert.ChangeType(item, typeof(T)));
            }

            return list;
        }
    }
}
