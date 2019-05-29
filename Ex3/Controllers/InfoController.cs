using Ex3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class InfoController : Controller
    {

        ClientModel client;
        private const int miliToSecond = 1000;
        FlightLogModel flightLogModel;
        private Timer timer;
        // GET: Info
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult display(string ip, int port, int interval) {
            //display/ip/port/ interval - optional
            System.Net.IPAddress iPAddress;
            if (System.Net.IPAddress.TryParse(ip, out iPAddress))
            {
                this.client = new ClientModel(ip, port, interval);
                this.client.PropertyChanged += this.Client_PropertyChanged;

                ViewBag.Interval = (int)((1 / (Double)interval) * 1000);
                ViewBag.Lon = Double.NaN;
                ViewBag.Lat = Double.NaN;

                return View();
            }
            else
            {
                //display/file name/interval
                string filePath = ip;
                int animationTime = port;
                this.flightLogModel = new FlightLogModel(filePath);
            }
        }
        [HttpGet]
        public ActionResult save(string ip, int port, int interval,int samplingTime,string fileName)
        {
            this.flightLogModel = new FlightLogModel(fileName);
            ActionResult actionResult = this.display(ip, port, interval);
            this.client.PropertyChanged += flightLogModel.PropertyChanged;
            this.timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = miliToSecond*samplingTime;
            timer.Enabled = true;
            return actionResult;
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("bye");
            this.client.PropertyChanged -= flightLogModel.PropertyChanged;
            this.timer.Enabled = false;
        }
        int i = 0;
        private void Client_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            i++;
            System.Diagnostics.Debug.WriteLine(i);
            ViewBag.Lon = client.Lon;
            ViewBag.Lat = client.Lat;
        }
    }
}