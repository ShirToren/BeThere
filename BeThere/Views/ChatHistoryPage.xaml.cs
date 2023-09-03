using BeThere.ViewModels.ChatViewModel;

namespace BeThere.Views;

public partial class ChatHistoryPage : ContentPage
{
	public ChatHistoryPage(ChatHistoryViewModel i_ChatHistoryViewModel)
	{
		InitializeComponent();
		BindingContext = i_ChatHistoryViewModel;
	}
}