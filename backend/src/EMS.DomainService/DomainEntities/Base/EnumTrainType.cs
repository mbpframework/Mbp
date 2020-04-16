using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Base
{
    public enum EnumTrainType
    {
        /// <summary>
        /// 军官共同训练
        /// </summary>
        JGGTXL = 1,
        /// <summary>
        /// 士兵共同训练
        /// </summary>
        SBGTXL = 2,
        /// <summary>
        /// 光端专业训练
        /// </summary>
        GDZYXL = 3,
        /// <summary>
        /// 军官专业训练
        /// </summary>
        JGZYXL = 4,
        /// <summary>
        /// 通信员专业训练
        /// </summary>
        TXYZYXL = 5,
        /// <summary>
        /// 光端战术训练
        /// </summary>
        GDZSXL = 6,
        /// <summary>
        /// 营连战术训练
        /// </summary>
        YLZSXL = 7,
        /// <summary>
        /// 部队训练
        /// </summary>
        BDXL = 8
    }
}
