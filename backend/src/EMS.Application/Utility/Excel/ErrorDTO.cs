using System;

namespace EMS.Application.Utility.Excel
{
    /// <summary>
    /// 错误信息DTO
    /// </summary>
    [Serializable]
    public class ErrorDTO
    {
        /// <summary>
        /// 错误信息DTO-构造方法1(无排序号)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="reason"></param>
        public ErrorDTO(string title, string reason)
        {
            this.Title = title;
            this.Reason = reason;
        }

        /// <summary>
        /// 错误信息DTO-构造方法2(有排序号)
        /// </summary>
        /// <param name="title"></param>
        /// <param name="reason"></param>
        /// <param name="sequence"></param>
        public ErrorDTO(string title, string reason, int sequence)
        {
            this.Title = title;
            this.Reason = reason;
            this.Sequence = sequence;
        }

        /// <summary>
        /// 错误信息DTO-错误标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 错误信息DTO-原因
        /// </summary>
        public string Reason { get; set; }


        /// <summary>
        /// 错误信息DTO-主键值
        /// </summary>
        public Guid MainGuidValue { get; set; }

        /// <summary>
        /// 错误信息DTO-排序号
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 错误信息DTO-行号
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 错误信息DTO-列名
        /// </summary>
        public string ColName { get; set; }
    }
}
