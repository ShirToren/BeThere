using BeThere.Models;
using BeThere.ViewModels.ChatViewModel;
using System.Collections.Generic;
using System.Text.Json;

namespace BeThere.Views;

public partial class ChatHistoryPage : ContentPage
{
    private readonly ChatHistoryViewModel r_ChatHistoryViewModel;
    private bool m_AppearingForFirstTime;
	public ChatHistoryPage(ChatHistoryViewModel i_ChatHistoryViewModel)
	{
		InitializeComponent();
		BindingContext = i_ChatHistoryViewModel;
        r_ChatHistoryViewModel = i_ChatHistoryViewModel;
        m_AppearingForFirstTime = true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if(m_AppearingForFirstTime)
        {
            if (Preferences.ContainsKey("MessagesList"))
            {
                string serializedList = Preferences.Get("MessagesList", string.Empty);
                List<ChatMessage> list = JsonSerializer.Deserialize<List<ChatMessage>>(serializedList);
                foreach (ChatMessage chatMessage in list)
                {
                    r_ChatHistoryViewModel.AvailableChats.Add(chatMessage);
                }
            }
            if (Preferences.ContainsKey("ChatRoomsList"))
            {
                string serializedList = Preferences.Get("ChatRoomsList", string.Empty);
                HashSet<string> list = JsonSerializer.Deserialize<HashSet<string>>(serializedList);
                foreach (string chatRoom in list)
                {
                    r_ChatHistoryViewModel.AvailableChatRooms.Add(chatRoom);
                }
            }
            m_AppearingForFirstTime = false;
        }
    }
}