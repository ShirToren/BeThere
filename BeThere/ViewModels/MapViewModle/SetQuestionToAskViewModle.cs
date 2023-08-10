using System;
using BeThere.Models;
using BeThere.Services;
using Plugin.LocalNotification;

using CommunityToolkit.Mvvm.ComponentModel;

namespace BeThere.ViewModels
{
    [QueryProperty(nameof(LocationToQuestion),"Location")]
    [QueryProperty(nameof(LocationAddress), "Address")]
    [QueryProperty(nameof(Radius), "Radius")]

    public partial class SetQestionToAskViewModle : BaseViewModels
	{
        private QuestionAskedService m_SendQuestionService;
        public Command SendQuestionCommand { get; }
        private QuestionToAsk m_QuestionToAsk;
  
        [ObservableProperty]
        Location locationToQuestion;

        [ObservableProperty]
        private string locationAddress;

        private double m_Radius;
        public double Radius { get => m_Radius; set { m_Radius = value; IsRadiusSet = m_Radius != 0; }}

        [ObservableProperty]
        bool isRadiusSet;


        public SetQestionToAskViewModle(QuestionAskedService i_SendQuestionService)
		{
            m_SendQuestionService = i_SendQuestionService;
            m_QuestionToAsk = new QuestionToAsk();
            SendQuestionCommand = new Command(async () => await sendQuestionClicked());
        }

        public QuestionToAsk QuestionToAsk { get { return m_QuestionToAsk; } }


        private async Task sendQuestionClicked()
        {
            if (IsBusy == true)
            {
                return;
            }

            try
            {
                IsBusy = true;
                setQuestionLocationDetails();

                ResultUnit<string> response = await m_SendQuestionService.TryPostNewQuestion(m_QuestionToAsk);

                sendNotification();

                if (response.IsSuccess == true)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                    
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

        private void setQuestionLocationDetails()
        {
            //m_QuestionToAsk.LocationLatitude = LocationToQuestion.Latitude;
            //m_QuestionToAsk.LocationLongitude = LocationToQuestion.Longitude;
            LocationData location = new LocationData(LocationToQuestion.Latitude, LocationToQuestion.Longitude);
            m_QuestionToAsk.Location = location; 
            m_QuestionToAsk.Date = LocationToQuestion.Timestamp.Date.ToString();
            m_QuestionToAsk.Time = LocationToQuestion.Timestamp.Hour.ToString();
            m_QuestionToAsk.Radius = m_Radius;
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

