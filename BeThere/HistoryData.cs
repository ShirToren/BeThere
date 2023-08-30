using BeThere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere
{
    public static class HistoryData
    {
        private static readonly Dictionary<string, QuestionToAsk> sr_AllQuestions = new Dictionary<string, QuestionToAsk>();
        private static readonly Dictionary<string, QuestionAnswers> sr_AllAnswers = new Dictionary<string, QuestionAnswers>();

        public static void AddQuestion(string i_QuestionId, QuestionToAsk i_Question)
        {
            sr_AllQuestions.Add(i_QuestionId, i_Question);
        }
        public static void AddNewAnswersItem(string i_QuestionId)
        {
            QuestionAnswers questionAnswers = new QuestionAnswers(i_QuestionId, new List<UserAnswer>());
            sr_AllAnswers.Add(i_QuestionId, questionAnswers);
        }
        public static void AddAnswersItem(string i_QuestionId, QuestionAnswers i_QuestionAnswers)
        {
            if(i_QuestionAnswers.userAnswers == null)
            {
                i_QuestionAnswers.userAnswers = new List<UserAnswer>();
            }
            sr_AllAnswers.Add(i_QuestionId, i_QuestionAnswers);
        }
        public static void AddSingelAnswer(string i_QuestionId, UserAnswer i_UserAnswer)
        {
            sr_AllAnswers[i_QuestionId].userAnswers.Add(i_UserAnswer);
        }
        public static List<UserAnswer> GetAnswersByQuestionId(string i_QuestionId)
        {
            if (sr_AllAnswers.ContainsKey(i_QuestionId))
            {
                return sr_AllAnswers[i_QuestionId].userAnswers;
            }
            else
            {
                return new List<UserAnswer>();
            }
        }
        public static QuestionToAsk GetQuestionObjectByQuestionId(string i_QuestionId)
        {
            if (sr_AllQuestions.ContainsKey(i_QuestionId))
            {
                return sr_AllQuestions[i_QuestionId];
            }
            else
            {
                return null;
            }
        }
    }
}
