using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Ex3.Models
{
    public class FlightLogModel
    {
        string fileName;
        public FlightLogModel(string fileName)
        {
            this.fileName = fileName;   
        }
        private void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //write data
            System.IO.File.AppendAllText(fileName, e.ToString());
        }

    }
}