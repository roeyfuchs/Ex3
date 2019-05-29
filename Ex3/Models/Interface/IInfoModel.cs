using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex3.Models.Interface {
    interface IInfoModel {
        event PropertyChangedEventHandler PropertyChanged;
        Tuple<double, double> getValues();
        double Lat { get; set; }
        double Lon { get; set; }
    }
}
