using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ex3.Models {
    public class InfoModel {
        private string ip;
        private int port;
        private TcpListener serv;
        private bool needStop = false;
        private const int bufferSize = 1024;
        const char msgSplitter = '\n';
        const char csvSplitter = ',';
        const int minInfo = 2;

        List<Tuple<double, double>> location;


        public InfoModel(string ip, int port) {
            this.ip = ip;
            this.port = port;
            Task serverTask = new Task(() => {
                this.Start();
            });
            serverTask.Start();
        }

        private void Start() {
            System.Diagnostics.Debug.WriteLine("STARTTTTTTTTTTT\n");
            this.serv = new TcpListener(IPAddress.Parse(this.ip), this.port);
            this.serv.Start();
            System.Diagnostics.Debug.WriteLine("Open TCP\n");
            TcpClient client = this.serv.AcceptTcpClient();
            System.Diagnostics.Debug.WriteLine("Connected TCP\n");
            NetworkStream ns = client.GetStream();

            while(!needStop) {
                byte[] msg = new byte[bufferSize];
                string strMsg = Encoding.Default.GetString(msg);
                strMsg = strMsg.Split(msgSplitter)[0].Trim();
                string[] words = strMsg.Split(csvSplitter);

                if(words.Length < minInfo) {
                    continue;
                }

                location.Add(new Tuple<double, double>(Double.Parse(words[0]), Double.Parse(words[1])));
                System.Diagnostics.Debug.WriteLine(Double.Parse(words[0]) +", " +  Double.Parse(words[1]));
            }

            client.Close();
            this.serv.Stop();
        }

        public void Stop() {
            this.needStop = true;
        }
    }
}