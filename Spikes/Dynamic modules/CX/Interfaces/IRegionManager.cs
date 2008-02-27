using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CX.Interfaces
{
    public interface IRegionManager : IServiceMarker
    {
        void AddRegion(IRegion region, string regionName);
        IRegion FindRegion(string regionName);
        IRegion this[string name] { get; }
    }
}
