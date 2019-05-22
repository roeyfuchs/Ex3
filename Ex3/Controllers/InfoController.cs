using Ex3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ex3.Controllers
{
    public class InfoController : Controller
    {

        ClientModel client;
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

            return View();
        }
        [HttpGet]
        public ActionResult save(string ip, int port, int interval,int samplingTime,string fileName)
        {
            ClientModel infoModel = new ClientModel(ip, port, interval);
            return View();
        }

        private void Client_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            ViewBag.Lon = client.Lon;
            ViewBag.Lat = client.Lat;
        }
    }
}