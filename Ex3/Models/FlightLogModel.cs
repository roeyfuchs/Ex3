using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Ex3.Models
{
    public class FlightLogModel
    {
        public event EventHandler SamplingCounter;
        public string FileName { get; set; }
        private static Queue<FlightDetails> fileInfo;
        private static FlightLogModel s_instace = null;
        private static int countSampling { set; get; }
     

        public static FlightLogModel Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new FlightLogModel();
                    countSampling = 0;
                }
                return s_instace;
            }
        }
    
        public void PropertyChanged(object sender, FlightDetailsEventArgs e)
        {
               BinaryFormatter bf = new BinaryFormatter();
               FileStream fsout = new FileStream(FileName, FileMode.Append, FileAccess.Write, FileShare.None);
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
            SamplingCounter?.Invoke(this, new EventArgs());
        }
        private void LoadFileData()
        {
            fileInfo = new Queue<FlightDetails>();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fsin = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                using (fsin)
                {
                    while (fsin.Position != fsin.Length)
                    {
                        fileInfo.Enqueue((FlightDetails)bf.Deserialize(fsin));
                    }
                }
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Error in deserializg");
            }
            //enqueue Nan Flight data
            fileInfo.Enqueue(EnququeNanFightData());
        }
        public FlightDetails EnququeNanFightData()
        {
            return new FlightDetails(Double.NaN, Double.NaN, Double.NaN, Double.NaN);
        }
        public string GetCurrentFlightDetails()
        {
            if (fileInfo == null)
            {
                LoadFileData();
            }
            FlightDetails currentData= fileInfo.Dequeue();
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("infos");
            currentData.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

    }
}