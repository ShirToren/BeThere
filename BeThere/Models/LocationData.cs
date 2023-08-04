using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.Models
{
    public class LocationData
    {
        public LocationData(double i_Latitude, double i_Longitude) 
        {
            this.Latitude = i_Latitude;
            this.Longitude = i_Longitude;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
