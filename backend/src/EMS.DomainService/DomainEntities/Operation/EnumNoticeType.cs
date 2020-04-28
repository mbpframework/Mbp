using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Operation
{
    public enum EnumNoticeType
    {
        /// <summary>
        /// 公告
        /// </summary>
        Notice = 1,
        /// <summary>
        /// 通告
        /// </summary>
        Announcement=2,
        /// <summary>
        /// 决议
        /// </summary>
        resolution=3,
        /// <summary>
        /// 公报
        /// </summary>
        Bulletin=4,
        /// <summary>
        /// 意见
        /// </summary>
        Opinion=5,
        /// <summary>
        /// 通报
        /// </summary>
        Circulate=6,
        /// <summary>
        /// 纪要
        /// </summary>
        Summary=7
    }
}
