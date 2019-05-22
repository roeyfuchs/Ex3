using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex3.Models.Interface {
    interface IInfoModel {
        double Lat { get; set; }
        double Lon { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    
    }
}
