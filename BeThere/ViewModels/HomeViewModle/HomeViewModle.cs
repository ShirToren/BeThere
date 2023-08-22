using System;
using BeThere;
using BeThere.Models;
using System.Collections.ObjectModel;
using BeThere.Services;
using BeThere.Views;
using Plugin.LocalNotification;


namespace BeThere.ViewModels
{
    public class UsersHistoryViewModle : BaseViewModels
    {
        private QuestionAskedService m_HistoryService;
        private UpdateLocationService m_UpdateLocationService;
        private NotificationsService m_NotificationService;
        private SharedDataSource m_SharedDataSource;

        public ObservableCollection<QuestionToAsk> UsersPreviousQuestions => m_SharedDataSource.UsersPreviousQuestions;

        public Command GoToDetailsCommand { get; }
        public Command AskNewQuestionCommand { get; }


        public UsersHistoryViewModle(QuestionAskedService i_HistoryService, UpdateLocationService i_UpdateLocationService, NotificationsService i_NotificationService, SharedDataSource i_SharedDateSource)
        {
            Title = "My questions";
            m_HistoryService = i_HistoryService;
            m_UpdateLocationService = i_UpdateLocationService;
            GoToDetailsCommand = new Command<QuestionToAsk>(async (question) => await GoToDetailsPage(question));
            AskNewQuestionCommand = new Command(async () => await GoToMapPage());
            Task task = GetAllPreviousQuestion();
            m_NotificationService = i_NotificationService;
            m_SharedDataSource = i_SharedDateSource;
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
            //await sendNotification();
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

                        if(KeyValue.Value.Item2 != null)
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
