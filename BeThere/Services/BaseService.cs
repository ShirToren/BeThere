using System;
using Newtonsoft.Json;
using System.Text;
using BeThere.Models;
//using Android.Service.Autofill;

namespace BeThere.Services
{
    public class BaseService
    {
        
        private static readonly string s_BaseUrl = "http://10.0.2.2:5209/";
       // private static readonly string s_BaseUrl = "https://betherserverapi.azurewebsites.net";
        private static HttpClient s_ServerClient;


        public static HttpClient GetHttpClient()
        {
            if (s_ServerClient is not null)
            {
                return s_ServerClient;
            }

            s_ServerClient = new HttpClient { BaseAddress = new Uri(s_BaseUrl) };
            return s_ServerClient;
        }


        public static StringContent ObjectToJsonBody(object i_objectToConvert)
        {
            string jsonData = JsonConvert.SerializeObject(i_objectToConvert);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            return content;
        }

        public static UserData ConnectedUser { get; set; }
    }
}
