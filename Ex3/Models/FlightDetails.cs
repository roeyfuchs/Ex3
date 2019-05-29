using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    [Serializable]
    public class FlightDetails
    {
        public double Lon { set; get; }
        public double Lat { set; get; }
        public double Throttle { set; get; }
        public double Rudder { set; get; }
        public FlightDetails(double lon, double lat, double throttle, double rudder)
        {
            this.Lat = lat;
            this.Lon = lon;
            this.Throttle = throttle;
            this.Rudder = rudder;
        }
    }
}