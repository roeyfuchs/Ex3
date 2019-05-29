using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    [Serializable]
    public class FlightDetails
    {
        public double lon { set; get; }
        public double lat { set; get; }
        public double throttle { set; get; }
        public double rudder { set; get; }
        public FlightDetails(double lon, double lat, double throttle, double rudder)
        {
            this.lat = lat;
            this.lon = lon;
            this.throttle = throttle;
            this.rudder = rudder;
        }
    }
}