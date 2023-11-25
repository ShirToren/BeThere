using CommunityToolkit.Mvvm.ComponentModel;
using System;
using BeThere.Views;
using BeThere.Services;
using BeThere.Models;

namespace BeThere.ViewModels
{
    public partial class ConnectToAppViewModle : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command RegisterWithFaceBookCommand { get; }
        private ConnectToAppService m_LoginService;
        private AuthonticationService m_AuthService;
        private ChatService m_ChatService;
        private QuestionAskedService m_HistoryService;
        private ConnectWithFacebook m_FaceBookConnect;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string loginFailureMessage;

        public ConnectToAppViewModle(ConnectToAppService i_LoginService, AuthonticationService i_AuthonticationService, QuestionAskedService i_HistoryService, ChatService i_ChatService)
        {
            
            m_LoginService = i_LoginService;
            m_AuthService = i_AuthonticationService;
            m_HistoryService = i_HistoryService;
            m_ChatService = i_ChatService;
            LoginCommand = new Command(async () => await loginClicked());
            RegisterCommand = new Command(async () => await registerClicked());
        }

        private async Task loginClicked()
        {
            if (IsBusy == true)
            {
                return;
            }

            try
            {
                LoginFailureMessage = string.Empty;
                IsBusy = true;
                if (UserName == null || UserName == String.Empty || Password == null || Password == String.Empty)
                {
                    await Application.Current.MainPage.DisplayAlert("Unable to login", "Please enter user name and password.", "OK");
                } 
                else
                {
                    ResultUnit<string> response = await m_LoginService.TryToLogin(UserName, Password);

                    if (response.IsSuccess == true)
                    {
                        m_AuthService.Login();
                        m_AuthService.StoreAuthenticatedUser(UserName);
                        await GetAllPreviousQuestion();
                        await LoadChats();
                        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                    }
                    else
                    {
                        handleIncorrectLogin(response.ResultMessage);
                    }
                }
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

        private async Task registerClicked()
        {
            UserName = string.Empty;
            Password = string.Empty;
            LoginFailureMessage = string.Empty;
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }

        private void handleIncorrectLogin(string i_resultMessage)
        {
            switch (i_resultMessage)
            {
                case "No user found":
                    UserName = string.Empty;
                    Password = string.Empty;
                    LoginFailureMessage = $"{i_resultMessage},try again or register";
                    break;
                case "Incorect password":
                    Password = string.Empty;
                    LoginFailureMessage = $"{i_resultMessage},try again";
                    break;
                default:
                    break;
            }
        }

        private async Task loginWithFacebookClicked()
        {

        }

        public async Task LoadChats()
        {
            var result = await m_ChatService.TryGetUserRooms();
            if (result.ReturnValue.Count > 0)
            {
                foreach (string room in result.ReturnValue)
                {
                    SharedDataSource.AvailableChatRooms.Add(room);
                    var messageResponse = await m_ChatService.TryGetLastMessageByChatRoomId(room);
                    if (messageResponse.ReturnValue != null)
                    {
                        SharedDataSource.AvailableChats.Add(messageResponse.ReturnValue);
                    }
                }
            }
        }

        public async Task GetAllPreviousQuestion()
        {
/*            if (IsBusy)
            {
                return;
            }*/

            try
            {

                IsBusy = true;
                ResultUnit<Dictionary<string, Tuple<QuestionToAsk, QuestionAnswers>>> response = await m_HistoryService.TryGetPreviousQuestions();


                if (response.IsSuccess == true)
                {
                    if (SharedDataSource.UsersPreviousQuestions.Count != 0)
                    {
                        SharedDataSource.UsersPreviousQuestions.Clear();
                    }

                    foreach (var KeyValue in response.ReturnValue)
                    {
                        HistoryData.AddQuestion(KeyValue.Key, KeyValue.Value.Item1);
                        SharedDataSource.UsersPreviousQuestions.Add(KeyValue.Value.Item1);

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
                //IsBusy = false;
            }
        }

    }
}
