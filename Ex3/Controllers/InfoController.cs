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
    public class InfoController : Controller
    {

        IInfoModel client;
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
            this.client = new ClientModel(ip, port, interval);
            this.client.PropertyChanged += this.Client_PropertyChanged;

            ViewBag.Interval =  (int)((1 / (Double)interval) * 1000);
            ViewBag.Lon = Double.NaN; 
            ViewBag.Lat = Double.NaN;

            return View();
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

        [HttpPost]
        public void SetValues() {
            /*Tuple<double, double> vals = client.getValues();
            ViewBag.Lon = vals.Item1;
            ViewBag.Lat = vals.Item2;*/
        }
    }
}