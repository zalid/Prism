using System.Collections.Generic;
using System.Globalization;
using Prism.Interfaces;

namespace Prism.Services
{
    public class RegionManagerService : IRegionManagerService
    {
        private readonly Dictionary<string, IRegion> _regions = new Dictionary<string, IRegion>();

        public void Register(string regionName, IRegion region)
        {
            _regions.Add(regionName, region);
        }

        public IRegion GetRegion(string regionName)
        {
            if (_regions.ContainsKey(regionName))
                return _regions[regionName];

            throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, "{0} not found", regionName));
        }

        public bool HasRegion(string regionName)
        {
            return _regions.ContainsKey(regionName);
        }
    }
}
