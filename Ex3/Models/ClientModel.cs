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
        private int waitingTime = 0;
        private bool needStop = false;
        private const int bufferSize = 1024;
        private const string lonCommand = "get /position/longitude-deg\r\n";
        private const string latCommand = "get /position/latitude-deg\r\n";

        public event PropertyChangedEventHandler PropertyChanged;


        public double Lat { get; set; }
        public double Lon { get; set; }


        public ClientModel(string ip, int port, int intervalPerSec) {
            System.Diagnostics.Debug.WriteLine("regular const");
            this.ip = ip;
            this.port = port;
            if(intervalPerSec != 0) {
                this.waitingTime = (int)((1 / (Double)intervalPerSec) * 1000);
            }
            Task serverTask = new Task(() => {
                this.Start();
            });
            serverTask.Start();
        }


        private void Start() {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse(this.ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, this.port);
            socket.Connect(remoteEP);
            while (!needStop) {
                socket.Send(System.Text.Encoding.ASCII.GetBytes(lonCommand));
                byte[] buffer = new byte[bufferSize];
                int iRx = socket.Receive(buffer);
                string recv = Encoding.ASCII.GetString(buffer, 0, iRx);
                this.Lon = fromSimToDobule(recv);
                System.Diagnostics.Debug.WriteLine("Lon = " + this.Lon);

                socket.Send(System.Text.Encoding.ASCII.GetBytes(latCommand));
                Array.Clear(buffer, 0, buffer.Length);
                iRx = socket.Receive(buffer);
                recv = Encoding.ASCII.GetString(buffer, 0, iRx);
                this.Lat = fromSimToDobule(recv);
                System.Diagnostics.Debug.WriteLine("Lat = " + this.Lat);

                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(this.Lat +","+ this.Lon));
                System.Diagnostics.Debug.WriteLine("wating time ==" + this.waitingTime);
                if (this.waitingTime == 0) {
                    break;
                }
                Thread.Sleep(this.waitingTime);

            }
            socket.Close();


        }

        public void Stop() {
            this.needStop = true;
        }


        private double fromSimToDobule(string str) {
            string onlyNum = Regex.Match(str, @"'(.*?[^\\])'").Value;
            onlyNum = onlyNum.Trim('\'');
            return Double.Parse(onlyNum);
        }
    }
}