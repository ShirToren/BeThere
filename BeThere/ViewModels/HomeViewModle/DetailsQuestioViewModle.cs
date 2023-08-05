using System;
using BeThere.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BeThere.ViewModels
{
    [QueryProperty(nameof(Question), "Question")]

    public partial class DetailsQuestionViewModle : BaseViewModels
    {

        public DetailsQuestionViewModle()
        {
            //m_question = new QuestionData();

        }

        [ObservableProperty]
        private QuestionToAsk question;

    }
}

