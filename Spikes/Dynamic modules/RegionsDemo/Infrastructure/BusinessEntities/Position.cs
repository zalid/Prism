using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.BusinessEntities
{
    public class Position
    {
        public int Latitude { get; set; }
        public int Longtitude { get; set; }

        public Position(int latitude, int longtitude)
        {
            this.Latitude = latitude;
            this.Longtitude = longtitude;
        }
    }
}
