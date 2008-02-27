using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;

namespace CX
{
    public class RegionManager : IRegionManager
    {
        private IDictionary<string, IRegion> regions = new Dictionary<string, IRegion>();

        public void AddRegion(IRegion region, string regionName)
        {
            if (!regions.ContainsKey(regionName))
                regions.Add(regionName, region);
        }

        public IRegion FindRegion(string regionName)
        {
            if (regions.ContainsKey(regionName))
                return regions[regionName];

            throw new NotSupportedException();
        }

        public IRegion this[string regionName]
        {
            get
            {
                if (regions.ContainsKey(regionName))
                    return regions[regionName];

                throw new NotSupportedException();
            }
        }
    }
}
