using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class FlightDetailsEventArgs : EventArgs
    {
        private const string comma = ",";
        private const string newLine = "\n";
        double LON { set; get; }
        double LAT { set; get; }
        new string ToString => LON + comma + LAT + newLine;
        public FlightDetailsEventArgs(double LON, double LAT)
        {
            this.LAT = LAT;
            this.LON = LON;
        }
    }
}