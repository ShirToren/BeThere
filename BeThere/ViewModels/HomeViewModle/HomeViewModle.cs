using System;
using BeThere.Models;
using System.Collections.ObjectModel;
using BeThere.Services;
using BeThere.Views;
using Plugin.LocalNotification;

namespace BeThere.ViewModels
{
    public class UsersHistoryViewModle : BaseViewModels
    {
        private ObservableCollection<QuestionToAsk> m_UsersPreviousQuestions;
        private QuestionAskedService m_HistoryService;
        private UpdateLocationService m_UpdateLocationService;

        public Command GoToDetailsCommand { get; }
        public Command AskNewQuestionCommand { get; }



        public UsersHistoryViewModle(QuestionAskedService i_HistoryService, UpdateLocationService i_UpdateLocationService)
        {
            Title = "Past questions";
            m_HistoryService = i_HistoryService;
            m_UpdateLocationService = i_UpdateLocationService;
            m_UsersPreviousQuestions = new ObservableCollection<QuestionToAsk>();
            GoToDetailsCommand = new Command<QuestionToAsk>(async (question) => await GoToDetailsPage(question));
            AskNewQuestionCommand = new Command(async () => await GoToMapPage());
            Task task = GetAllPreviousQuestion();

        }

        public ObservableCollection<QuestionToAsk> PreviousQuestions { get { return m_UsersPreviousQuestions; } }

        public async Task GoToDetailsPage(QuestionToAsk i_QuestionTappted)
        {
            if (i_QuestionTappted is null)
            {
                return;
            }


            var navigationParameter = new Dictionary<string, object>
            {
                ["Question"] = i_QuestionTappted,
            };

            await Shell.Current.GoToAsync($"{nameof(DetailsQuestionPage)}", navigationParameter);
        }

        public async Task GoToMapPage()
        {
             sendNotification();
            await Shell.Current.GoToAsync(nameof(MapPage));
        }

        private async void sendNotification()
        {
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }

            var notification = new NotificationRequest
            {
                NotificationId = 100,
                Title = "New question",
                Description = "description",
                ReturningData = "Dummy data", // Returning data when tapped on notification.
                Schedule =
    {
        NotifyTime = DateTime.Now // Used for Scheduling local notification, if not specified notification will show immediately.
    }
            };

            await LocalNotificationCenter.Current.Show(notification);
        }


        public async Task GetAllPreviousQuestion()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                string userName = "User";
                IsBusy = true;
                ResultUnit<List<QuestionToAsk>> response = await m_HistoryService.TryGetPreviousQuestions(userName);
                if (m_UsersPreviousQuestions.Count != 0)
                {
                    m_UsersPreviousQuestions.Clear();
                }

                if (response.IsSuccess == true)
                {
                    foreach (QuestionToAsk previousQuestion in response.ReturnValue)
                    {
                        m_UsersPreviousQuestions.Add(previousQuestion);
                    }
                }
                else
                {
                    //no prev questions?
                }
            }
            catch
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}

