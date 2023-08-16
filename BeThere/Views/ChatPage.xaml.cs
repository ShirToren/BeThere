namespace BeThere.Views;

using BeThere.ViewModels.ChatViewModel;
using Microsoft.AspNetCore.SignalR.Client;

public partial class ChatPage : ContentPage
{
    //private readonly HubConnection _connection;
    public ChatPage(ChatViewModel i_ChatViewModel)
	{
		InitializeComponent();
        BindingContext = i_ChatViewModel;

/*        var baseUrl = "http://localhost";

        // Android can't connect to localhost
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            baseUrl = "http://10.0.2.2";
        }

        _connection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}:5209/chat")
            .Build();*/


/*        _connection = new HubConnectionBuilder()
            .WithUrl("http://10.0.2.2:5209/chat")
            .Build();*/

/*        _connection.On<string>("MessageReceived", (message) =>
        {
            Dispatcher.Dispatch(new Action(() =>
            {
                chatMessages.Text += $"{Environment.NewLine}{message}";
            }));
            
        });*/

/*

        Task.Run(() =>
        {
            Dispatcher.Dispatch(async () =>
            await _connection.StartAsync());
        });*/
    }

    /*    private async void OnSendClicked(object sender, EventArgs e)
        {
            await _connection.InvokeCoreAsync("SendMessage", args: new[] {
                "User name: ",
                myChatMessage.Text });

            myChatMessage.Text = String.Empty;
        }*/
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}