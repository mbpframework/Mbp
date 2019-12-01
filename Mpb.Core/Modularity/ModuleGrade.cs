using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Modularity
{
    public enum EnumModuleGrade
    {
        /// <summary>
        /// 核心模块
        /// </summary>
        Core = 1,

        /// <summary>
        /// 组件级别模块,这种模块可以替换,比如MQ,可以是ActiveMQ,也可以是RabbitMQ
        /// </summary>
        Component = 10,

        /// <summary>
        /// 应用级别，就是我们通常说的功能模块,领域驱动设计中的子域
        /// </summary>
        Application = 20
    }
}
