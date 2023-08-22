namespace BeThere.Views;

using BeThere.ViewModels.ChatViewModel;
using Microsoft.AspNetCore.SignalR.Client;

public partial class ChatPage : ContentPage
{
    private readonly ChatViewModel r_ChatViewModel;
    public ChatPage(ChatViewModel i_ChatViewModel)
	{
		InitializeComponent();
        BindingContext = i_ChatViewModel;
        r_ChatViewModel = i_ChatViewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnDisappearing()
    {
        // Perform actions when the page is about to disappear
        // This could include cleanup, saving data, etc.
        r_ChatViewModel.clearMessages();
        base.OnDisappearing();
    }
}