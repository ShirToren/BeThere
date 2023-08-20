using BeThere.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.Services
{
    public class AnswerService : BaseService
    {
        public async Task<ResultUnit<string>> TryPostNewAnswer(UserAnswer i_Answer)
        {
            ResultUnit<string> result = new ResultUnit<string>();
            string endPointQueryUri = $"api/Answer?UserName={ConnectedUser.Username}";

            string jsonData = JsonConvert.SerializeObject(i_Answer);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await GetHttpClient().PostAsync(endPointQueryUri, content);
            if (!response.IsSuccessStatusCode)
            {
                //handle ba
            }

            return result;
        }
    }
}
