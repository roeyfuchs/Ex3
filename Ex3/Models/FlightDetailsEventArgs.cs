using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class FlightDetailsEventArgs : EventArgs
    {
        public FlightDetails FlightDetails { get; set; }
        /// <summary>
        /// constuctor
        /// </summary>
        /// <param name="flightDetails"></param>
        public FlightDetailsEventArgs(FlightDetails flightDetails)
        {
            this.FlightDetails = flightDetails;
        }
    }
}