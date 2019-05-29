using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace Ex3.Models
{
    public class FlightLogModel
    {
        string fileName;
        private const char comma = ',';
        Queue<FlightDetails> fileInfo;
        public FlightLogModel(string fileName)
        {
            this.fileName = fileName;
            fileInfo = new Queue<FlightDetails>();
        }
        public void PropertyChanged(object sender, FlightDetailsEventArgs e)
        {
            //write data
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fsout = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            try
            {
                using (fsout)
                {
                    bf.Serialize(fsout, e.FlightDetails);
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error in serialzing");
            }
        }
        public void LoadFileData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fsin = new FileStream("employee.binary", FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                using (fsin)
                {
                    this.fileInfo = (Queue<FlightDetails>)bf.Deserialize(fsin);
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error in deserializg");
            }
        }
        public FlightDetails GetCurrentFlightDetails()
        {
            return this.fileInfo.Dequeue();
        }

    }
}