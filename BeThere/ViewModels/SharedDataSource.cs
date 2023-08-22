using BeThere.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.ViewModels
{
    public class SharedDataSource
    {
        public ObservableCollection<QuestionToAsk> UsersPreviousQuestions { get; } = new ObservableCollection<QuestionToAsk>();
        public ObservableCollection<UserAnswer> CurrentUserAnswers { get; } = new ObservableCollection<UserAnswer>();

    }
}
