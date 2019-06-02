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
        private readonly object lockQueue = new object();

        /// <summary>
        /// singelton object
        /// </summary>
        public static FlightLogModel Instance
        {
            get
            {
                if (s_instace == null)
                {
                    s_instace = new FlightLogModel();
                }
                return s_instace;
            }
        }
        /// <summary>
        /// serilize flightDetail object to given file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            //invoke counter
            SamplingCounter?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// deserilize flightDetail objects into queue from given file
        /// </summary>
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
        /// <summary>
        /// create null flight detail at the end of queue
        /// </summary>
        /// <returns></returns>
        public FlightDetails EnququeNanFightData()
        {
            return new FlightDetails(Double.NaN, Double.NaN, Double.NaN, Double.NaN);
        }
        /// <summary>
        /// return current fight detail
        /// </summary>
        /// <returns></returns>
        public string GetCurrentFlightDetails()
        {

            lock (lockQueue)
            {
                //if quque empty or simulation is over, reload data
                if (fileInfo == null|| fileInfo.Count==0) {
                LoadFileData();
                }
                FlightDetails currentData = fileInfo.Dequeue();
                //return flight data as xml node
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
}