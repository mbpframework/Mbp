using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json;

namespace Mbp.Ddd.Application.Mbp.Dto
{
    /// <summary>
    /// Dto基础类
    /// </summary>
    public abstract class DtoBase
    {
        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
