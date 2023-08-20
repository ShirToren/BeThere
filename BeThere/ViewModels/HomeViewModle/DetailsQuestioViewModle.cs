using System;
using System.Collections.ObjectModel;
using BeThere.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BeThere.ViewModels
{
    [QueryProperty(nameof(Question), "Question")]
    [QueryProperty(nameof(QuestionAskedAddress), "QuestionAddress")]

    public partial class DetailsQuestionViewModle : BaseViewModels
    {
        private ObservableCollection<QuestionToAsk> test;

        public DetailsQuestionViewModle()
        {
            test = new ObservableCollection<QuestionToAsk>();
            QuestionToAsk question1 = new QuestionToAsk();
            question1.Question = "Try";
            question1.Gender = "all";
            question1.Time = "12:12";
            QuestionToAsk question2 = new QuestionToAsk();
            question2.Question = "lala";
            question2.Gender = "all";
            question2.Time = "13:13";
            test.Add(question1);
            test.Add(question2);
      
        }

        [ObservableProperty]
        private QuestionToAsk question;

        [ObservableProperty]
        private string questionAskedAddress;

        public ObservableCollection<QuestionToAsk> Answers { get { return test; } }

    }
}
