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
using BeThere.ViewModels;
using Mopups.Services;

namespace BeThere.Services
{
    public class NotificationsService : BaseService
    {
        private readonly Timer r_Timer;
        IPopupNavigation popupNavigation;
        private static readonly Object sr_ListLock = new object();
        private AnswerService m_AnswerService;
        private ChatService m_ChatService;
        private CreditsService m_CreditsService;
        public NotificationsService(IPopupNavigation popupNavigation, AnswerService i_AnswerService, ChatService i_ChatService, CreditsService i_CreditsService)
        {
            this.popupNavigation = popupNavigation;
            r_Timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Notifications = new List<QuestionToAsk>();
            m_AnswerService = i_AnswerService;
            m_ChatService = i_ChatService;
            m_CreditsService = i_CreditsService;
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        }

        private async void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
            if (e.IsTapped)
            {
                string returningData = e.Request.ReturningData;
                if(returningData.Equals("Question"))
                {
                    ///push popup if not pushed
                }
                else
                {
                    QuestionToAsk question = HistoryData.GetQuestionObjectByQuestionId(returningData);
                    String LocationAddress = new String(await question.Location.GetLocationAdress());
                    var navigationParameter = new Dictionary<string, object>
                    {
                        ["Question"] = question,
                        ["QuestionAddress"] = LocationAddress.ToString(),
                    };

                    try
                    {
                        await MopupService.Instance.PopAsync();
                    } 
                    catch (Exception ex)
                    {

                    }
                    await Shell.Current.GoToAsync($"{nameof(DetailsQuestionPage)}", navigationParameter);
                }
            }
            if (e.IsDismissed)
            {
                try
                {
                    await MopupService.Instance.PopAsync();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private async void TimerCallback(object state)
        {
            ResultUnit<List<QuestionToAsk>> response = await TryGetNotifications();
            ResultUnit<List<UserAnswer>> answersResponse = await TryGetNewAnswers();

            lock (sr_ListLock)
            {
                Notifications = response.ReturnValue;
                NewAnswers = answersResponse.ReturnValue;
            }
            if(response.ReturnValue.Count > 0)
            {
                await sendQuestionNotification();
                await popupNavigation.PushAsync((Mopups.Pages.PopupPage)new PopupQuestionPage(response.ReturnValue[0], m_AnswerService, m_ChatService, m_CreditsService));
            }
            if(answersResponse.ReturnValue.Count > 0)
            {
                foreach(UserAnswer answer in answersResponse.ReturnValue)
                {
                    HistoryData.AddSingelAnswer(answer.QuestionId, answer);
                    //if details page is showing this question, add new answers directly to current answers list on shared data
                    if (SharedDataSource.CurrentQuestion != null && (SharedDataSource.CurrentQuestion.QuestionId.Equals(answer.QuestionId)))
                    {
                        SharedDataSource.CurrentUserAnswers.Add(answer);
                    }
                    await sendAnswerNotification();
                    await popupNavigation.PushAsync((Mopups.Pages.PopupPage)new PopupAnswerPage(answer, m_AnswerService, m_ChatService));
                }
            }
        }

        public async Task<ResultUnit<List<QuestionToAsk>>> TryGetNotifications()
        {
            ResultUnit<List<QuestionToAsk>> result = new ResultUnit<List<QuestionToAsk>>();
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

        private async Task sendQuestionNotification()
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
                        ReturningData = "Question", // Returning data when tapped on notification.
                        Schedule =
    {
        NotifyTime = DateTime.Now.AddSeconds(10) // Used for Scheduling local notification, if not specified notification will show immediately.
    }
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }
                
            }
        }

        private async Task sendAnswerNotification()
        {

            await LocalNotificationCenter.Current.RequestNotificationPermission();

            lock (sr_ListLock)
            {
                foreach (UserAnswer userAnswer in NewAnswers)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = 100,
                        Title = "New answer",
                        Description = "from " + userAnswer.Username,
                        ReturningData = userAnswer.QuestionId, // Returning data when tapped on notification.
                        Schedule =
    {
        NotifyTime = DateTime.Now.AddSeconds(10) // Used for Scheduling local notification, if not specified notification will show immediately.
    }
                    };

                    LocalNotificationCenter.Current.Show(notification);
                }

            }
        }

        public async Task<ResultUnit<List<UserAnswer>>> TryGetNewAnswers()
        {
            ResultUnit<List<UserAnswer>> result = new ResultUnit<List<UserAnswer>>();
            string username = ConnectedUser.Username;
            string endPointQueryUri = $"api/Answer?UserName={username}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<UserAnswer> newAnswers = JsonConvert.DeserializeObject<List<UserAnswer>>(jsonResponse);
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
        public List<UserAnswer> NewAnswers { get; set; }
    }
}
