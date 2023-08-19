using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeThere.Models;
using Plugin.LocalNotification;
using Mopups.Interfaces;
using BeThere.Views;



namespace BeThere.Services
{
    public class NotificationsService : BaseService
    {
        private readonly Timer r_Timer;
        IPopupNavigation popupNavigation;
        private static readonly Object sr_ListLock = new object();
        private AnswerService m_AnswerService;
        public NotificationsService(IPopupNavigation popupNavigation, AnswerService i_AnswerService)
        {
            this.popupNavigation = popupNavigation;
            r_Timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Notifications = new List<QuestionToAsk>();
            m_AnswerService = i_AnswerService;
        }

        private async void TimerCallback(object state)
        {
            ResultUnit<List<QuestionToAsk>> response = await TryGetNotifications();
            ResultUnit<List<Answer>> answersResponse = await TryGetNewAnswers();

            lock (sr_ListLock)
            {
                Notifications = response.ReturnValue;
                NewAnswers = answersResponse.ReturnValue;
            }
            if(response.ReturnValue.Count > 0)
            {
                await sendNotification();
                await popupNavigation.PushAsync(new PopupPage(Notifications[0], m_AnswerService));
            }
            if(answersResponse.ReturnValue.Count > 0)
            {
                string shir = String.Empty;
            }
        }

        public async Task<ResultUnit<List<QuestionToAsk>>> TryGetNotifications()
        {
            ResultUnit<List<QuestionToAsk>> result = new ResultUnit<List<QuestionToAsk>>();
            //string username = ConnectedUser.Username;
            string username = ConnectedUser.Username;
            string endPointQueryUri = $"api/Notifications?UserName={username}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<QuestionToAsk> notifications = JsonConvert.DeserializeObject<List<QuestionToAsk>>(jsonResponse);
                result.ReturnValue = notifications;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.IsSuccess = false;
                result.ResultMessage = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException();
            }

            return result;

        }

        private async Task sendNotification()
        {
            //if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            ///{
            ///
            await LocalNotificationCenter.Current.RequestNotificationPermission();
            //Dispatcher dispatcher = (Dispatcher)Dispatcher.GetForCurrentThread();
            //dispatcher.Dispatch(async () => await LocalNotificationCenter.Current.RequestNotificationPermission());
            //}
        
            lock (sr_ListLock)
            {
                foreach (QuestionToAsk questionToAsk in Notifications)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = 100,
                        Title = "New question",
                        Description = questionToAsk.Question,
                        ReturningData = "Dummy data", // Returning data when tapped on notification.
                        Schedule =
    {
        NotifyTime = DateTime.Now // Used for Scheduling local notification, if not specified notification will show immediately.
    }
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }
                
            }
        }

        public async Task<ResultUnit<List<Answer>>> TryGetNewAnswers()
        {
            ResultUnit<List<Answer>> result = new ResultUnit<List<Answer>>();
            string username = ConnectedUser.Username;
            string endPointQueryUri = $"api/Answer?UserName={username}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<Answer> newAnswers = JsonConvert.DeserializeObject<List<Answer>>(jsonResponse);
                result.ReturnValue = newAnswers;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.IsSuccess = false;
                result.ResultMessage = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException();
            }

            return result;

        }

        public List<QuestionToAsk> Notifications { get; set; }
        public List<Answer> NewAnswers { get; set; }
    }
}
