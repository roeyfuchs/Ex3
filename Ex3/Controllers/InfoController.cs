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
        /// <summary>
        /// return Index view
        /// </summary>
        /// <returns>Action result</returns>
        public ActionResult Index() {
            return View();
        }

       /// <summary>
       /// display- get flight information from the server or from text file
       /// </summary>
       /// <param name="ip"></param>
       /// <param name="port"></param>
       /// <param name="interval"></param>
       /// <returns>action result</returns>
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
                client.Start();
                this.client = client;
                //save info on view bag
                this.client.PropertyChanged += this.Client_PropertyChanged;
               
                ViewBag.Lon = Double.NaN;
                ViewBag.Lat = Double.NaN;
            } else {
                //display/file name/interval
                string filePath = ip;
                interval = port;
                runAnimation = true;
                this.flightLogModel = FlightLogModel.Instance;
                this.flightLogModel.FileName = filePath;
                interval = port; //no port
            }
            ViewBag.Interval = (int)((1 / (Double)interval) * 1000);
            return View();
        }
        /// <summary>
        /// save flight information
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="interval"></param>
        /// <param name="samplingTime"></param>
        /// <param name="fileName"></param>
        /// <returns>action result</returns>
        [HttpGet]
        public ActionResult save(string ip, int port, int interval, int samplingTime, string fileName) {
            this.flightLogModel = FlightLogModel.Instance;
            this.flightLogModel.FileName = fileName;
            runAnimation = false;
            //update sampling object
            samplingData = new SamplingData(interval * samplingTime);
            ActionResult actionResult = this.display(ip, port, interval);
            //assign documanting method
            this.client.PropertyChanged += flightLogModel.PropertyChanged;
            //assign counter
            flightLogModel.SamplingCounter += this.CountSamplingRaised;
            return actionResult;
        }
        /// <summary>
        /// stop sampling data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// update view bag with flight data from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_PropertyChanged(object sender, FlightDetailsEventArgs e) {
            ViewBag.Lon = e.FlightDetails.Lon;
            ViewBag.Lat = e.FlightDetails.Lat;
            ViewBag.rudder = e.FlightDetails.Rudder;
            ViewBag.throttle = e.FlightDetails.Throttle;
        }
        /// <summary>
        /// return current flight data- from server or text file
        /// </summary>
        /// <returns>return xml string</returns>
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
                //get values from the server
                ClientModel cl = ClientModel.Instance;
                string str = cl.GetValues();
                return str;
            };
        }
    }
}