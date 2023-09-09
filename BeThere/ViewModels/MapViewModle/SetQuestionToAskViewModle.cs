using System;
using BeThere.Models;
using BeThere.Services;
using Plugin.LocalNotification;

using CommunityToolkit.Mvvm.ComponentModel;

namespace BeThere.ViewModels
{
    [QueryProperty(nameof(LocationToQuestion), "Location")]
    [QueryProperty(nameof(LocationAddress), "Address")]
    [QueryProperty(nameof(Radius), "Radius")]
    [QueryProperty(nameof(IsCityQuestion), "isCity")]

    public partial class SetQestionToAskViewModle : BaseViewModel
    {

        private QuestionAskedService m_SendQuestionService;
        private ChatService m_ChatService;
        public Command SendQuestionCommand { get; }
        public Command ClearAllCommand { get; }
        private QuestionToAsk m_QuestionToAsk;

        [ObservableProperty]
        Location locationToQuestion;

        [ObservableProperty]
        private string locationAddress;

        private double m_Radius;
        public double Radius { get => m_Radius; set { m_Radius = value; IsRadiusSet = m_Radius != 0; } }

        private bool m_IsCityQuestion;
        public bool IsCityQuestion { get => m_IsCityQuestion; set { m_IsCityQuestion = value; } }

        [ObservableProperty]
        bool isRadiusSet;


        public SetQestionToAskViewModle(QuestionAskedService i_SendQuestionService, ChatService i_ChatService)
        {
            m_SendQuestionService = i_SendQuestionService;
            m_ChatService = i_ChatService;
            m_QuestionToAsk = new QuestionToAsk();
            SendQuestionCommand = new Command(async () => await sendQuestionClicked());
            ClearAllCommand = new Command(() => clearAllClicked());
        }

        //public QuestionToAsk QuestionToAsk { get { return m_QuestionToAsk; }
            public QuestionToAsk QuestionToAsk
            {
                get => m_QuestionToAsk;
                set => SetProperty(ref m_QuestionToAsk, value);
            }

        
        private void clearAllClicked()
        {
            QuestionToAsk = new QuestionToAsk();
        }


        private async Task sendQuestionClicked()
        {
            if (IsBusy == true)
            {
                return;
            }

            try
            {
                IsBusy = true;
                if (m_QuestionToAsk.Question == null || m_QuestionToAsk.Question.Equals(String.Empty))
                {
                    await Application.Current.MainPage.DisplayAlert("Unable to send question", "Text is empty.", "OK");
                }
                else
                {
                   await setQuestionLocationDetails();

                    Guid guidForQuestionId = Guid.NewGuid();
                    Guid guidForChatRoomId = Guid.NewGuid();

                    m_QuestionToAsk.QuestionId = guidForQuestionId.ToString();
                    m_QuestionToAsk.ChatRoomId = guidForChatRoomId.ToString();

                    ResultUnit<string> response = await m_SendQuestionService.TryPostNewQuestion(m_QuestionToAsk);

                    HistoryData.AddQuestion(m_QuestionToAsk.QuestionId, m_QuestionToAsk);
                    HistoryData.AddNewAnswersItem(m_QuestionToAsk.QuestionId);

                    SharedDataSource.UsersPreviousQuestions.Add(m_QuestionToAsk);

                    if (response.IsSuccess == true)
                    {
                        QuestionToAsk = new QuestionToAsk();
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }

        }

        private async Task setQuestionLocationDetails()
        {
            LocationData location = new LocationData(LocationToQuestion.Latitude, LocationToQuestion.Longitude);
            m_QuestionToAsk.Location = location;
            m_QuestionToAsk.Date = LocationToQuestion.Timestamp.Date.ToString("dd/MM/yyyy");
            m_QuestionToAsk.Time = LocationToQuestion.Timestamp.TimeOfDay.ToString(@"hh\:mm");
            m_QuestionToAsk.Radius = m_Radius;
            m_QuestionToAsk.IsCityQuestion = IsCityQuestion;
            await m_QuestionToAsk.Location.InitializeCity();
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
                Description = m_QuestionToAsk.Question,
                ReturningData = "Dummy data", // Returning data when tapped on notification.
                Schedule =
    {
        NotifyTime = DateTime.Now // Used for Scheduling local notification, if not specified notification will show immediately.
    }
            };

            await LocalNotificationCenter.Current.Show(notification);
        }

    }
}