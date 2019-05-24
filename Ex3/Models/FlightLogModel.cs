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
        public void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //write data
            System.IO.File.AppendAllText(fileName,e.PropertyName+Environment.NewLine);
        }
       /* public Tuple<double,double> ReadData()
        {
            const char splitter = ',';
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Re
            }
        }*/

    }
}