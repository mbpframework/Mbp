using Mbp.Core.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain
{
    public interface IDemoDomainService : IDomainService
    {
        void Say();
    }
}
