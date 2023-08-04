﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using BeThere.Views;
using BeThere.Services;

namespace BeThere.ViewModels
{
    public partial class ConnectToAppViewModle : BaseViewModels
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public Command RegisterWithFaceBookCommand { get; }
        private ConnectToAppService m_LoginService;
        private ConnectWithFacebook m_FaceBookConnect;

        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string loginFailureMessage;

        public ConnectToAppViewModle(ConnectToAppService i_LoginService)
        {
            Title = "Login!!";
            m_LoginService = i_LoginService;
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
                ResultUnit<string> response = await m_LoginService.TryToLogin(UserName, Password);
                if (response.IsSuccess == true)
                {
                    await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
                }
                else
                {
                    handleIncorrectLogin(response.ResultMessage);
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

    }
}
