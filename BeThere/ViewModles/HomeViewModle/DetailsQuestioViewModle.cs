using System;
using BeThere.Models;

namespace BeThere.ViewModels
{
    [QueryProperty(nameof(Question), "Question")]
    public class DetailsQuestionViewModle : BaseViewModels
    {
        private QuestionToAsk m_question;

        public DetailsQuestionViewModle()
        {
            //m_question = new QuestionData();

        }

        public QuestionToAsk Question { get { return m_question; } }

    }
}

