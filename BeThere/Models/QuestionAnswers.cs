using System;
namespace BeThere.Models
{
	public class QuestionAnswers
	{
        public string questionId { get; set; }
        public List<UserAnswer> userAnswers { get; set; }
        public QuestionAnswers(string i_QuestionId, List<UserAnswer> i_AnswersList)
        {
            questionId = i_QuestionId;
            userAnswers = i_AnswersList;
        }
    }
}

