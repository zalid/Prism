using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.BusinessEntities;

namespace RestaurantModule.Services
{
    public class RestaurantLocationProviderService : ILocationProviderService
    {
        #region ILocationProviderService Members

        public Dictionary<Position, string> GetLocations()
        {
            Dictionary<Position, string> locations = new Dictionary<Position, string>();
            locations.Add(new Position(1, 1), "(Seattle) Freddie's Fish Stand");
            locations.Add(new Position(3, 3), "(Los Angeles) Jose's Tacos");

            return locations;
        }

        #endregion
    }
}
