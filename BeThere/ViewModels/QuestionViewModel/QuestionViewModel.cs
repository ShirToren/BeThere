using BeThere.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.ViewModels.QuestionViewModel
{
    public partial class QuestionViewModel : BaseViewModels
    {
        [ObservableProperty]
        private QuestionToAsk questionObject;

        [ObservableProperty]
        private int numOfAnswers;
    }
}
