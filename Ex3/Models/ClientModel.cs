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
        Socket socket;
        private const int bufferSize = 1024;
        private const string lonCommand = "get /position/longitude-deg\r\n";
        private const string latCommand = "get /position/latitude-deg\r\n";
        public event PropertyChangedEventHandler PropertyChanged;

        public Tuple<double, double> getValues() {
            double lat = getLat();
            double lon = getLon();
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(lat + "," + lon));
            return new Tuple<double, double>(lon, lat);
        }

        private double getLat() {
            return getInfo(latCommand);
        }
        private double getLon() {
            return getInfo(lonCommand);
        }

        private double getInfo(string str) {
            socket.Send(System.Text.Encoding.ASCII.GetBytes(str));
            byte[] buffer = new byte[bufferSize];
            int iRx = socket.Receive(buffer);
            string recv = Encoding.ASCII.GetString(buffer, 0, iRx);
            return fromSimToDobule(recv);
        }
        

        
        public double Lat { get; set; }
        public double Lon { get; set; }
        
        public ClientModel(string ip, int port, int intervalPerSec) {
            this.ip = ip;
            this.port = port;
            this.Start();
        }

        private void Start() {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse(this.ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, this.port);
            socket.Connect(remoteEP);
        }

        public void Stop() {
            socket.Close();
        }


        private double fromSimToDobule(string str) {
            string onlyNum = Regex.Match(str, @"'(.*?[^\\])'").Value;
            onlyNum = onlyNum.Trim('\'');
            return Double.Parse(onlyNum);
        }

    }
}