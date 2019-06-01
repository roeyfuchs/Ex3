using Ex3.Models;
using Ex3.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class InfoController : Controller {

        IInfoModel client;
        private static bool runAnimation;
        private const int miliToSecond = 1000;
        FlightLogModel flightLogModel;
        private SamplingData samplingData;


        // GET: Info
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        public ActionResult display(string ip, int port, int interval) {
            //display/ip/port/ interval - optional
            System.Net.IPAddress iPAddress;
            if (System.Net.IPAddress.TryParse(ip, out iPAddress)) {
                runAnimation = false;
                ClientModel client = ClientModel.Instance;
                client.Reset();
                client.Ip = ip;
                client.Port = port;
                client.Interval = (int)((1 / (Double)interval) * 1000);
                client.Start();
                this.client = client;
                
                this.client.PropertyChanged += this.Client_PropertyChanged;
               
                ViewBag.Interval = (int)((1 / (Double)interval) * 1000);
                ViewBag.Lon = Double.NaN;
                ViewBag.Lat = Double.NaN;
                ViewBag.sec = interval;
            } else {
                //display/file name/interval
                string filePath = ip;
                int animationTime = port;
                runAnimation = true;
                this.flightLogModel = FlightLogModel.Instance;
                this.flightLogModel.FileName = filePath;
            }
            return View();
        }
        [HttpGet]
        public ActionResult save(string ip, int port, int interval, int samplingTime, string fileName) {
            this.flightLogModel = FlightLogModel.Instance;
            runAnimation = true;
            this.flightLogModel.FileName = fileName;
            samplingData = new SamplingData(interval * samplingTime);
            ActionResult actionResult = this.display(ip, port, interval);
            this.client.PropertyChanged += flightLogModel.PropertyChanged;
            flightLogModel.SamplingCounter += this.CountSamplingRaised;
            return actionResult;
        }
        private void CountSamplingRaised(object sender,EventArgs e)
        {
            if (this.samplingData != null)
            {
                if (!samplingData.Sample())
                {
                    samplingData = null;
                    client.PropertyChanged -= flightLogModel.PropertyChanged;
                }
            }
        }
        private void Client_PropertyChanged(object sender, FlightDetailsEventArgs e) {
            ViewBag.Lon = e.FlightDetails.Lon;
            ViewBag.Lat = e.FlightDetails.Lat;
            ViewBag.rudder = e.FlightDetails.Rudder;
            ViewBag.throttle = e.FlightDetails.Throttle;
        }

        [HttpPost]
        public string SetValues() {
            if (runAnimation)
            {
                //flight animation
                FlightLogModel fl = FlightLogModel.Instance;
                return fl.GetCurrentFlightDetails();

            }
            else
            {
                ClientModel cl = ClientModel.Instance;
                string str = cl.GetValues();
                 System.Diagnostics.Debug.WriteLine(str);
                return str;
            };
        }
    }
}