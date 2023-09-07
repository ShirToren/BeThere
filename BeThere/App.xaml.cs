using BeThere.ViewModels.ChatViewModel;
using System.Text.Json;

namespace BeThere;

public partial class App : Application
{
    private ChatHistoryViewModel m_ChatHistoryViewModel;

    public App(ChatHistoryViewModel i_ChatHistoryViewModel)
	{
		InitializeComponent();

		MainPage = new AppShell();
        m_ChatHistoryViewModel = i_ChatHistoryViewModel;
    }
    protected override void OnSleep()
    {
        // This method is called when the app is about to be suspended or closed
        // You can save data to preferences or perform any cleanup here
        string serializedList = JsonSerializer.Serialize(m_ChatHistoryViewModel.AvailableChats);
        Preferences.Set("MessagesList", serializedList);

        string serializedList2 = JsonSerializer.Serialize(m_ChatHistoryViewModel.AvailableChatRooms);
        Preferences.Set("ChatRoomsList", serializedList2);
    }
}

