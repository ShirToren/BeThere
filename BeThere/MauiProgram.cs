using BeThere.Views;
using BeThere.Services;
using BeThere.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Plugin.LocalNotification;
using Mopups.Hosting;
using Mopups.Interfaces;
using Mopups.Services;
using BeThere.ViewModels.PopupViewModel;
using BeThere.ViewModels.ChatViewModel;

namespace BeThere;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseLocalNotification()
            .ConfigureMopups()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
                fonts.AddFont("Sitka.ttc", "Sitka");
            });


        //.ConfigureMauiHandlers(handlers =>
        //{
        //    handlers.AddHandler(typeof(Microsoft.Maui.Controls.Maps.Map), typeof(MapHandler));
        //});

#if DEBUG
        //builder.Logging.AddDebug();
#endif

        //builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<MapPage>();
        builder.Services.AddSingleton<ChatPage>();
        builder.Services.AddTransient<DetailsQuestionPage>();
        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddSingleton<ProfilePage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegisterPage>();
        builder.Services.AddSingleton<SetQuestionPage>();
        builder.Services.AddSingleton<IPopupNavigation>(MopupService.Instance);
        builder.Services.AddTransient<MainPage>();
       

        builder.Services.AddTransient<AuthonticationService>();
        builder.Services.AddSingleton<BaseService>();
        builder.Services.AddSingleton<QuestionAskedService>();
        builder.Services.AddSingleton<ConnectToAppService>();
        builder.Services.AddSingleton<UpdateLocationService>();
        builder.Services.AddSingleton<NotificationsService>();
        //builder.Services.AddSingleton<SendQuestionService>();
        builder.Services.AddSingleton<ChatService>();
        builder.Services.AddSingleton<AnswerService>();


        builder.Services.AddSingleton<ConnectToAppViewModle>();
        builder.Services.AddSingleton<ProfileViewModle>();
        builder.Services.AddTransient<SetQestionToAskViewModle>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddSingleton<UsersHistoryViewModle>();
        builder.Services.AddTransient<DetailsQuestionViewModle>();
        builder.Services.AddSingleton<PopupViewModel>();
        builder.Services.AddSingleton<ChatViewModel>();

        return builder.Build();
    }
}
