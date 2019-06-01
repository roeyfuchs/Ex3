using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

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

        public void ToXml(XmlWriter writer) {
            writer.WriteStartElement("info");
            writer.WriteElementString("Lon", this.Lon.ToString());
            writer.WriteElementString("Lat", this.Lat.ToString());
            writer.WriteElementString("Throttle", this.Throttle.ToString());
            writer.WriteElementString("Rudder", this.Rudder.ToString());
            writer.WriteEndElement();
        }

        public void AddToString(ref string lon, ref string lat, ref string thr, ref string rud) {
            if (lon != string.Empty) {
                lon += ",";
                lat += ",";
                thr += ",";
                rud += ",";
            }
            lon += this.Lon.ToString();
            lat += this.Lat.ToString();
            thr += this.Throttle.ToString();
            rud += this.Rudder.ToString();
        }


    }
}