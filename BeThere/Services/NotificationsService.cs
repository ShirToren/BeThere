using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeThere.Models;
using Plugin.LocalNotification;

namespace BeThere.Services
{
    public class NotificationsService : BaseService
    {
        private readonly Timer r_Timer;
        public NotificationsService()
        {
            r_Timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        private async void TimerCallback(object state)
        {
            ResultUnit<List<QuestionToAsk>> response = await TryGetNotifications();
            if(response.ReturnValue.Count > 0)
            {
                await sendNotification();
            }
        }

        public async Task<ResultUnit<List<QuestionToAsk>>> TryGetNotifications()
        {
            ResultUnit<List<QuestionToAsk>> result = new ResultUnit<List<QuestionToAsk>>();
            //string username = ConnectedUser.Username;
            string username = "User";
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
            await LocalNotificationCenter.Current.RequestNotificationPermission();
            //}

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
    }
}
