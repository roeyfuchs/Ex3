using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class FlightDetails
    {
        private const string comma = ",";
        private const string newLine = "\n";
        public double LON { set; get; }
        public double LAT { set; get; }
        public new string ToString => LON + comma + LAT+newLine;
    }
}