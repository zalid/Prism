using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.Interfaces
{
    public interface IRegionManagerServiceScopeProvider
    {
        IRegionManagerService RegionManagerService { get; }
    }
}
