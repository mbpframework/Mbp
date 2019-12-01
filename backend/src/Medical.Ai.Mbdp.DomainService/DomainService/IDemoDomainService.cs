using Mbp.Core.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Domain
{
    public interface IDemoDomainService : IDomainService
    {
        void Say();
    }
}
