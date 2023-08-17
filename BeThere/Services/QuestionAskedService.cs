﻿using BeThere.Models;
using Newtonsoft.Json;
using System.Text;

namespace BeThere.Services
{
    public class QuestionAskedService : BaseService
    {
        private List<QuestionToAsk> m_usersPreviousQuestion;

        public QuestionAskedService()
        {
            m_usersPreviousQuestion = new List<QuestionToAsk>();
        }

        public async Task<ResultUnit<List<QuestionToAsk>>> TryGetPreviousQuestions()
        {
            ResultUnit<List<QuestionToAsk>> result = new ResultUnit<List<QuestionToAsk>>();
            string endPointQueryUri = $"api/QuestionsAsked?UserName={ConnectedUser.Username}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                m_usersPreviousQuestion = JsonConvert.DeserializeObject<List<QuestionToAsk>>(jsonResponse);
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
            string endPointQueryUri = $"api/QuestionsAsked?UserName={ConnectedUser.Username}";

            string jsonData = JsonConvert.SerializeObject(i_Question);
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

