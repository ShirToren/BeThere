using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BeThere.Models;
using BeThere.Services;
using BeThere.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BeThere.ViewModels
{
    [QueryProperty(nameof(Question), "Question")]
    [QueryProperty(nameof(QuestionAskedAddress), "QuestionAddress")]

    public partial class DetailsQuestionViewModle : BaseViewModel
    {
        private ChatService m_ChatService;
        public ICommand ChatCommand { get; private set; }
        private ObservableCollection<UserAnswer> CurrentUserAnswers => SharedDataSource.CurrentUserAnswers;


        public DetailsQuestionViewModle(ChatService i_ChatService)
        {
            ChatCommand = new Command<UserAnswer>(chatClicked);
            m_ChatService = i_ChatService;
        }

        private async void chatClicked(UserAnswer item)
        {
            if (IsBusy == true)
            {
                return;
            }

            try
            {
                await m_ChatService.JoinChatRoom(item.ChatRoomId);
                await GoToChatPage(item.ChatRoomId);

                IsBusy = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("hey");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void UpdateAnswersList()
        {
            CurrentUserAnswers.Clear();
            List<UserAnswer> userAnswers = HistoryData.GetAnswersByQuestionId(Question.QuestionId);
            foreach (UserAnswer userAnswer in userAnswers)
            {
                CurrentUserAnswers.Add(userAnswer);
            }
        }

        public async Task GoToChatPage(string i_ChatRoomId)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                ["ChatRoomId"] = i_ChatRoomId
            };
            await Shell.Current.GoToAsync($"{nameof(ChatPage)}", navigationParameter);
        }

        [ObservableProperty]
        private QuestionToAsk question;

        [ObservableProperty]
        private string questionAskedAddress;

        public ObservableCollection<UserAnswer> Answers { get { return CurrentUserAnswers; } }

    }
}
