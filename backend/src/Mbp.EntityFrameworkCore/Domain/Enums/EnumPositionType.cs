using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.Domain.Enums
{
    /// <summary>
    /// 岗位类别
    /// </summary>
    public enum EnumPositionType
    {
        /// <summary>
        /// 指挥军官
        /// </summary>
        Commanders = 1,
        /// <summary>
        /// 技术军官
        /// </summary>
        TechnicalOfficer = 2,
        /// <summary>
        /// 技师
        /// </summary>
        Technician = 3,
        /// <summary>
        /// 领班员
        /// </summary>
        Foreman = 4,
        /// <summary>
        /// 值机员
        /// </summary>
        Operator = 5,
        /// <summary>
        /// 通信员
        /// </summary>
        Corresponden = 6
    }
}
