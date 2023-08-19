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

        public DetailsQuestionViewModle()
        {

            QuestionToAsk test1 = new QuestionToAsk();
            test1.Question = "Try";
            test1.Gender = "all";
            test1.Time = "12:12";
            QuestionToAsk test2 = new QuestionToAsk();
            test2.Question = "lala";
            test2.Gender = "all";
            test2.Time = "13:13";
            Answers.Add(test1);
            Answers.Add(test2);
        }

        [ObservableProperty]
        private QuestionToAsk question;

        [ObservableProperty]
        private string questionAskedAddress;


        private ObservableCollection<QuestionToAsk> test = new ObservableCollection<QuestionToAsk> { };

        public ObservableCollection<QuestionToAsk> Answers { get { return test; } }

    }
}
