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
                foreach(KeyValuePair<string, Tuple<QuestionToAsk, QuestionAnswers>> keyValue in m_usersPreviousQuestion)
                {
                    string endPointQueryUriForList = $"api/Questions/List?QuestionId={keyValue.Key}";
                    HttpResponseMessage responseForList = await GetHttpClient().GetAsync(endPointQueryUriForList);
                    if (responseForList.IsSuccessStatusCode)
                    {
                        string jsonResponseForList = await responseForList.Content.ReadAsStringAsync();
                        List<UserAnswer> list = JsonConvert.DeserializeObject<List<UserAnswer>>(jsonResponseForList);
                        keyValue.Value.Item2.userAnswers = list;
                    }
                }
               
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

        public async Task<ResultUnit<string>> TryPostNewQuestion(QuestionToAsk i_Question)
        {
            ResultUnit<string> result = new ResultUnit<string>();
            string endPointQueryUri = $"api/Questions?UserName={ConnectedUser.Username}";

            string jsonData = JsonConvert.SerializeObject(i_Question);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await GetHttpClient().PostAsync(endPointQueryUri, content);
            string questionId= await response.Content.ReadAsStringAsync();
            result.ReturnValue = questionId;

            //result.ReturnValue = long.Parse(response.Content.ToString());
            if (!response.IsSuccessStatusCode)
            {
                //handle ba
            }

            return result;
        }

    }
}

