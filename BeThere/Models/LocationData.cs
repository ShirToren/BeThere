using System;
using System.Collections.Generic;
using System.Globalization;
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
            InitializeCity();
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string City { get; set; }

        private async void InitializeCity()
        {
            City = await GetLocationCity();
        }

        public async Task<string> GetLocationAdress()
        {
            try
            {
                string locationAdress = null;
                var placemarks = await Geocoding.GetPlacemarksAsync(Latitude, Longitude);

                if (placemarks != null && placemarks.Any())
                {
                    var placemark = placemarks.FirstOrDefault();
                    locationAdress = $"{placemark.Thoroughfare}, {placemark.Locality}, {placemark.CountryName}";
                }

                return locationAdress;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

        public async Task<string> GetLocationCity()
        {
            try
            {
                string locationCity = null;
                var placemarks = await Geocoding.GetPlacemarksAsync(Latitude, Longitude);
                if (placemarks != null && placemarks.Any())
                {
                    var placemark = placemarks.FirstOrDefault();
                    locationCity = placemark.Locality;
                }

                return locationCity;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }
    }

}
