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
        int i=0;
        public FlightLogModel(string fileName)
        {
            this.fileName = fileName;   
        }
        public void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //write data
            i++;
            System.Diagnostics.Debug.WriteLine("ser: "+i);
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