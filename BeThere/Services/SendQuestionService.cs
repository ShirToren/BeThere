using System;
using BeThere.Models;

using Newtonsoft.Json;

namespace BeThere.Services
{
	public class SendQuestionService : BaseService
	{
		public SendQuestionService()
		{
		}

        public async Task<ResultUnit<string>> TryPostNewQuestion(string i_Username, QuestionToAsk i_Question)
        {
            ResultUnit<string> result = new ResultUnit<string>();
            string endPointQueryUri = $"CreateAccount";

            //string jsonData = JsonConvert.SerializeObject(i_UserData);
            //StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await GetClient().PostAsync(endPointQueryUri, content);
            //if (response.IsSuccessStatusCode)
            //{
            //    result.IsSuccess = true;
            //}
            //else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    result.IsSuccess = false;
            //    result.ResultMessage = await response.Content.ReadAsStringAsync();
            //}
            //else
            //{
            //    throw new HttpRequestException();
            //}

            return result;
        }
    }
}

