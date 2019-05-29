using Ex3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ex3.Models.Interface {
    public delegate void infoHandel(object sender, FlightDetailsEventArgs e);

    interface IInfoModel {
        event infoHandel PropertyChanged;
        void GetValues();
        double Lat { get; set; }
        double Lon { get; set; }
    }
}
