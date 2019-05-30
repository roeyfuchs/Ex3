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
        private const char comma = ',';
        Queue<FlightDetails> fileInfo;

        private static FlightLogModel s_instace = null;



        public static FlightLogModel Instance {
            get {
                if (s_instace == null) {
                    s_instace = new FlightLogModel();
                }
                return s_instace;
            }
        }

        private FlightLogModel()
        {
            fileInfo = new Queue<FlightDetails>();
        }

        public string FileName { set; get; }

        public void PropertyChanged(object sender, FlightDetailsEventArgs e)
        {
            //write data
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fsout = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
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