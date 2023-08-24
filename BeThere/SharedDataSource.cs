using BeThere.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere
{
    public static class SharedDataSource
    {
        public static ObservableCollection<QuestionToAsk> UsersPreviousQuestions { get; } = new ObservableCollection<QuestionToAsk>();
        public static ObservableCollection<UserAnswer> CurrentUserAnswers { get; } = new ObservableCollection<UserAnswer>();
        public static ObservableCollection<ChatMessage> CurrentChatMessages { get; } = new ObservableCollection<ChatMessage>();
    }
}
