using Ex3.Models.Interface;
using System;
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
        public event infoHandel PropertyChanged;
        private static ClientModel s_instace = null;

        

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
            double lat = GetLat();
            double lon = GetLon();
            double throttle = GetThrottle();
            double rudder = GetRudder();
            FlightDetails flightDetails = new FlightDetails(lon, lat, throttle, rudder);
            this.PropertyChanged?.Invoke(this, new FlightDetailsEventArgs(flightDetails));
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("infos");
            flightDetails.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
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
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse(this.Ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, this.Port);
            this.socket.Connect(remoteEP);
        }

        public void Stop() {
            this.socket.Close();
        }


        private double FromSimToDobule(string str) {
            string onlyNum = Regex.Match(str, @"'(.*?[^\\])'").Value;
            onlyNum = onlyNum.Trim('\'');
            return Double.Parse(onlyNum);
        }

    }
}