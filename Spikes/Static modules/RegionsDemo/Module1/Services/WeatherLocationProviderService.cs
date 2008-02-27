using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.BusinessEntities;

namespace WeatherModule.Services
{
    public class WeatherLocationProviderService : ILocationProviderService
    {
        #region ILocationProviderService Members

        public Dictionary<Position, string> GetLocations()
        {
            Dictionary<Position, string> locations = new Dictionary<Position, string>();
            locations.Add(new Position(1, 1), "(Seattle) Rainy");
            locations.Add(new Position(2, 2), "(New Delhi) Hot");

            return locations;
        }

        #endregion
    }
}
