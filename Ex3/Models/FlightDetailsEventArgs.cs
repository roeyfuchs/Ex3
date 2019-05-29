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
        public FlightDetails FlightDetails { get; set; }
        public FlightDetailsEventArgs(FlightDetails flightDetails)
        {
            this.FlightDetails = flightDetails;
        }
    }
}