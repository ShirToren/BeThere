using System;
using BeThere.Views;

namespace BeThere;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        //Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage)); //gives a string of chatpage to navigate.
        //Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(ChatHistoryPage), typeof(ChatHistoryPage));
        Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));

        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(DetailsQuestionPage), typeof(DetailsQuestionPage));
        Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        Routing.RegisterRoute(nameof(SetQuestionPage), typeof(SetQuestionPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));


    }
}
