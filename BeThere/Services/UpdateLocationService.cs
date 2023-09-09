using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Http;
using BeThere.Models;

//using Windows.Media.Protection.PlayReady;

namespace BeThere.Services
{
    public class UpdateLocationService : BaseService
    {
        private readonly Timer r_Timer;

        public UpdateLocationService()
        {
            //r_Timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            SendHttpRequest();
        }

        private void TimerCallback(object state)
        {
            // Perform the HTTP request here.
            SendHttpRequest();

            // You can add additional logic or processing as needed.
        }

        private async void SendHttpRequest()
        {

            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;

                    // Now you can send this location data to your server using an HTTP request.
                    // Replace the following line with your HTTP request code to send the location data.
                    //Console.WriteLine($"Latitude: {latitude}, Longitude: {longitude}");
                    LocationData locationToPost = new LocationData(latitude, longitude);
                    await locationToPost.InitializeCity();
                    
                    ResultUnit<string> result = await TryPostCurrentLocation(locationToPost);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while trying to get the location
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    double latitude = location.Latitude;
                    double longitude = location.Longitude;

                    // Now you can send this location data to your server using an HTTP request.
                    // Replace the following line with your HTTP request code to send the location data.
                    Console.WriteLine($"Latitude: {latitude}, Longitude: {longitude}");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while trying to get the location
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task <ResultUnit<string>> TryPostCurrentLocation(LocationData i_Location)
        {
            ResultUnit<string> result = new ResultUnit<string>();
            string endPointQueryUri = $"api/UpdateLocation?UserName={ConnectedUser.Username}";
            string jsonData = JsonConvert.SerializeObject(i_Location);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await GetHttpClient().PostAsync(endPointQueryUri, content);

            if (!response.IsSuccessStatusCode)
            {
                result.IsSuccess = true;
            }

            return result;
        }

    }

}
