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

    public partial class DetailsQuestionViewModle : BaseViewModels
    {
        //private ObservableCollection<UserAnswer> answers;
        private ChatService m_ChatService;
        private SharedDataSource m_SharedDataSource;
        public ICommand ChatCommand { get; private set; }
        public ObservableCollection<UserAnswer> CurrentUserAnswers => m_SharedDataSource.CurrentUserAnswers;


        public DetailsQuestionViewModle(ChatService i_ChatService, SharedDataSource i_SharedDateSource)
        {
            //answers = new ObservableCollection<UserAnswer>();
            ChatCommand = new Command<UserAnswer>(chatClicked);
            m_ChatService = i_ChatService;
            m_SharedDataSource = i_SharedDateSource;
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
