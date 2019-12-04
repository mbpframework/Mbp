using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Mbp.Ddd.Application.System.Linq
{
    /// <summary>
    /// IQueryable扩展
    /// </summary>
    public static class MbpQueryableExtensions
    {
        /// <summary>
        /// 分页查询,升序
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="source">待分页数据源</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码数,从1开始</param>
        /// <param name="total">总数</param>
        /// <param name="whereLambda">谓词表达式</param>
        /// <param name="orderbyLambda">排序字段</param>
        /// <returns></returns>
        public static IQueryable<T> PageByAscending<T, TKey>(this IQueryable<T> source, int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderbyLambda)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            total = source.Where(whereLambda).Count();

            var temp = source.Where(whereLambda)
                         .OrderBy<T, TKey>(orderbyLambda)
                         .Skip(pageSize * (pageIndex - 1))
                         .Take(pageSize);
            return temp.AsQueryable();
        }

        /// <summary>
        /// 分页查询,降序
        /// </summary>
        /// <typeparam name="T">数据源类型</typeparam>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="source">待分页数据源</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页码数,从1开始</param>
        /// <param name="total">总数</param>
        /// <param name="whereLambda">谓词表达式</param>
        /// <param name="orderbyLambda">排序字段</param>
        /// <returns></returns>
        public static IQueryable<T> PageByDescending<T, TKey>(this IQueryable<T> source, int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderbyLambda)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            total = source.Where(whereLambda).Count();

            var temp = source.Where(whereLambda)
                       .OrderByDescending<T, TKey>(orderbyLambda)
                       .Skip(pageSize * (pageIndex - 1))
                       .Take(pageSize);
            return temp.AsQueryable();
        }
    }
}
