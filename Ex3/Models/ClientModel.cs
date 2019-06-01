using Ex3.Models.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Ex3.Models {
    public class ClientModel : IInfoModel {
        private Socket socket;
        private const int bufferSize = 1024;
        private const string lonCommand = "get /position/longitude-deg\r\n";
        private const string latCommand = "get /position/latitude-deg\r\n";
        private const string throttleCommand = "get /controls/engines/current-engine/throttle\r\n";
        private const string rudderCommand = "get /controls/flight/rudder\r\n";
        private bool online = true;

       
        public event infoHandel PropertyChanged;
        private static ClientModel s_instace = null;
        private BlockingCollection<FlightDetails> qt = new BlockingCollection<FlightDetails>();




        public static ClientModel Instance {
            get {
                if (s_instace == null) {
                    s_instace = new ClientModel();
                }
                return s_instace;
            }
        }

        public string Ip { set; get; }
        public int Port { set; get; }

        public string GetValues() {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("infos");
            string Lon, Lat, Throttle, Rudder;
            Lon = Lat = Throttle = Rudder = string.Empty;
            bool haveElement = true;
            while (haveElement) {
                System.Diagnostics.Debug.WriteLine(this.qt.Count);
                FlightDetails fd;
                fd = this.qt.Take();
                fd.AddToString(ref Lon, ref Lat, ref Throttle, ref Rudder);
                if(this.qt.Count == 0) {
                    haveElement = false;
                }
            }
            writer.WriteStartElement("info");
            writer.WriteElementString("Lon", Lon);
            writer.WriteElementString("Lat", Lat);
            writer.WriteElementString("Throttle", Throttle);
            writer.WriteElementString("Rudder", Rudder);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        public int Interval { get; set; }


        public void RefreshData() {
            double lat = GetLat();
            double lon = GetLon();
            double throttle = GetThrottle();
            double rudder = GetRudder();
            FlightDetails flightDetails = new FlightDetails(lon, lat, throttle, rudder);
            this.PropertyChanged?.Invoke(this, new FlightDetailsEventArgs(flightDetails));
            this.qt.Add(flightDetails);
        }

        private double GetLat() {
            return GetInfo(latCommand);
        }
        private double GetLon() {
            return GetInfo(lonCommand);
        }
        private double GetThrottle() {
            return GetInfo(throttleCommand);
        }
        private double GetRudder() {
            return GetInfo(rudderCommand);
        }

        private double GetInfo(string str) {
            this.socket.Send(System.Text.Encoding.ASCII.GetBytes(str));
            byte[] buffer = new byte[bufferSize];
            int iRx = socket.Receive(buffer);
            string recv = Encoding.ASCII.GetString(buffer, 0, iRx);
            return FromSimToDobule(recv);
        }

        public double Lat { get; set; }
        public double Lon { get; set; }

       

        public void Start() { 
            Task t = new Task(() => {
                this.ClientRunning();
            });
            t.Start();
        }

        private void ClientRunning() {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse(this.Ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, this.Port);
            this.socket.Connect(remoteEP);
            while (this.online) {
                this.RefreshData();
                Thread.Sleep(this.Interval);
            }
            this.socket.Close();
        }

        public void Stop() {
            this.online = false;
        }

        public void Reset() {
            if (this.socket != null) {
                this.Stop();
            }
        }


        private double FromSimToDobule(string str) {
            string onlyNum = Regex.Match(str, @"'(.*?[^\\])'").Value;
            onlyNum = onlyNum.Trim('\'');
            return Double.Parse(onlyNum);
        }

    }
}