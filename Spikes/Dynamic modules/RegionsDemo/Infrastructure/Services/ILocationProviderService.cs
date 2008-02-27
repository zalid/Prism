using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CX.Interfaces;
using Infrastructure.BusinessEntities;

namespace Infrastructure.Services
{
    public interface ILocationProviderService : IServiceMarker
    {
        Dictionary<Position, string> GetLocations();
    }
}
