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

namespace Ex3.Models {
    public class ClientModel : IInfoModel {
        private string ip;
        private int port;
        private Socket socket;
        private const int bufferSize = 1024;
        private const string lonCommand = "get /position/longitude-deg\r\n";
        private const string latCommand = "get /position/latitude-deg\r\n";
        private const string throttleCommand = "get /controls/engines/current-engine/throttle\r\n";
        private const string rudderCommand = "get /controls/flight/rudder\r\n";
        public event infoHandel PropertyChanged;

        public void GetValues() {
            double lat = GetLat();
            double lon = GetLon();
            double throttle = GetThrottle();
            double rudder = GetRudder();
            this.PropertyChanged?.Invoke(this, new FlightDetailsEventArgs(new FlightDetails(lon, lat, throttle, rudder)));
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
        
        public ClientModel(string ip, int port, int intervalPerSec) {
            this.ip = ip;
            this.port = port;
            this.Start();
        }

        private void Start() {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse(this.ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, this.port);
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