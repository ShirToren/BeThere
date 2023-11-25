using System;
using BeThere;
using BeThere.Models;
using System.Collections.ObjectModel;
using BeThere.Services;
using BeThere.Views;
using Plugin.LocalNotification;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BeThere.ViewModels
{
    public partial class HomeViewModle : BaseViewModel
    {
        private QuestionAskedService m_HistoryService;
        private UpdateLocationService m_UpdateLocationService;
        private NotificationsService m_NotificationService;
        private ChatService m_ChatService;

        public ObservableCollection<QuestionToAsk> UsersPreviousQuestions => SharedDataSource.UsersPreviousQuestions;

        public Command GoToDetailsCommand { get; }
        public Command AskNewQuestionCommand { get; }

        [ObservableProperty]
        private string helloMessage;


        public HomeViewModle(QuestionAskedService i_HistoryService, UpdateLocationService i_UpdateLocationService, NotificationsService i_NotificationService, ChatService i_ChatService)
        {
            Title = "My questions";
            m_HistoryService = i_HistoryService;
            m_UpdateLocationService = i_UpdateLocationService;
            GoToDetailsCommand = new Command<QuestionToAsk>(async (question) => await GoToDetailsPage(question));
            AskNewQuestionCommand = new Command(async () => await GoToMapPage());
            Task task = GetAllPreviousQuestion();
            m_NotificationService = i_NotificationService;
            m_ChatService = i_ChatService;
            HelloMessage = "Hello, " + LogedInUser.LogedInUserName();
            Task.Run(() =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                await m_ChatService.StartConnection());
            });
        }


        public async Task GoToDetailsPage(QuestionToAsk i_QuestionTappted)
        {
            if (i_QuestionTappted is null)
            {
                return;
            }
          
            String LocationAddress = new String(await i_QuestionTappted.Location.GetLocationAdress());

            var navigationParameter = new Dictionary<string, object>
            {
                ["Question"] = i_QuestionTappted,
                ["QuestionAddress"] = LocationAddress.ToString(),
            };

            await Shell.Current.GoToAsync($"{nameof(DetailsQuestionPage)}", navigationParameter);
        }

        public async Task GoToMapPage()
        {
            await Shell.Current.GoToAsync(nameof(MapPage));
        }


        public async Task GetAllPreviousQuestion()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {

                IsBusy = true;
                ResultUnit<Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>>> response = await m_HistoryService.TryGetPreviousQuestions();


                if (response.IsSuccess == true)
                {
                    if (UsersPreviousQuestions.Count != 0)
                    {
                        UsersPreviousQuestions.Clear();
                    }

                        foreach (var KeyValue in response.ReturnValue)
                        {
                            HistoryData.AddQuestion(KeyValue.Key, KeyValue.Value.Item1);
                            UsersPreviousQuestions.Add(KeyValue.Value.Item1);

                            if (KeyValue.Value.Item2 != null)
                            {
                                HistoryData.AddAnswersItem(KeyValue.Key, KeyValue.Value.Item2);
                            }
                            else
                            {
                                HistoryData.AddNewAnswersItem(KeyValue.Key);
                            }
                        }
                    
                }

                else
                {
                    //no prev questions?
                }
            }
            catch (Exception ex)
            {
                string massage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
