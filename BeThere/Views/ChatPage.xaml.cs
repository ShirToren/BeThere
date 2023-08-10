namespace BeThere.Views;
using Microsoft.AspNetCore.SignalR.Client;

public partial class ChatPage : ContentPage
{
    private readonly HubConnection _connection;
    public ChatPage()
	{
		InitializeComponent();
        _connection = new HubConnectionBuilder()
            .WithUrl("http://10.0.2.2:5209/chat")
            .Build();

        _connection.On<string>("MessageReceived", (message) =>
        {
            Dispatcher.Dispatch(new Action(() =>
            {
                chatMessages.Text += $"{Environment.NewLine}{message}";
            }));
            
        });

        Task.Run(() =>
        {
            Dispatcher.Dispatch(async () =>
            await _connection.StartAsync());
        });
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        await _connection.InvokeCoreAsync("SendMessage", args: new[] { myChatMessage.Text });

        myChatMessage.Text = String.Empty;
    }
}