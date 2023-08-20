using BeThere.Models;
using Newtonsoft.Json;
using System.Text;

namespace BeThere.Services
{
    public class QuestionAskedService : BaseService
    {
        private Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>> m_usersPreviousQuestion;

        public QuestionAskedService()
        {
            m_usersPreviousQuestion = new Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>>();
        }

        public async Task<ResultUnit<Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>>>> TryGetPreviousQuestions()
        {
            ResultUnit<Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>>> result = new ResultUnit<Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>>>();
            string endPointQueryUri = $"api/Questions?UserName={ConnectedUser.Username}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                m_usersPreviousQuestion = JsonConvert.DeserializeObject<Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>>>(jsonResponse);
                result.ReturnValue = m_usersPreviousQuestion;
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

        public async Task<ResultUnit<long>> TryPostNewQuestion(QuestionToAsk i_Question)
        {
            ResultUnit<long> result = new ResultUnit<long>();
            string endPointQueryUri = $"api/Questions?UserName={ConnectedUser.Username}";

            string jsonData = JsonConvert.SerializeObject(i_Question);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await GetHttpClient().PostAsync(endPointQueryUri, content);
            string questionId= await response.Content.ReadAsStringAsync();
            result.ReturnValue = long.Parse(questionId);

            //result.ReturnValue = long.Parse(response.Content.ToString());
            if (!response.IsSuccessStatusCode)
            {
                //handle ba
            }

            return result;
        }

    }
}

