using Newtonsoft.Json;
using BeThere.Models;
using System.Text;
using Xamarin.Essentials;


namespace BeThere.Services
{
    public class ConnectToAppService : BaseService
    {

        public async Task<ResultUnit<string>> TryToLogin(string i_Username, string i_Password)
        {
            ResultUnit<string> result = new ResultUnit<string>();

                 string endPointQueryUri = $"Login?UserName={i_Username}&Password={i_Password}";
           
                 HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);


                if (response.IsSuccessStatusCode)
            {
                result.IsSuccess = true;
                // Read the response content as a string
                string jsonResponse = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON string into MyData object using Newtonsoft.Json
                UserData userData = JsonConvert.DeserializeObject<UserData>(jsonResponse);

                if(userData != null) 
                {
                    ConnectedUser = userData;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.IsSuccess = false;
                result.ResultMessage = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException();
            }
                

            return result;
        }

        public async Task<ResultUnit<string>> TryToRegisterUser(UserData i_UserData)
        {
            ResultUnit<string> result = new ResultUnit<string>();
            string endPointQueryUri = $"CreateAccount";

            string jsonData = JsonConvert.SerializeObject(i_UserData);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await GetHttpClient().PostAsync(endPointQueryUri, content);
            if (response.IsSuccessStatusCode)
            {
                result.IsSuccess = true;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.IsSuccess = false;
                result.ResultMessage = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException();
            }

            return result;
        }





    }
}
