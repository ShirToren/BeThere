using BeThere.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using BeThere.Models;

namespace BeThere.ViewModels.ChatViewModel
{
    [QueryProperty(nameof(ChatRoomId), "ChatRoomId")]
    public partial class ChatViewModel : BaseViewModels
    {
        private readonly ChatService r_ChatLogic;
        public Command SendMessageCommand { get; }

        [ObservableProperty]
        private string message;
        [ObservableProperty]
        private string allMessages;
        public string ChatRoomId { get; set; }

        public ChatViewModel(ChatService i_ChatLogic)
        {
            Title = "Chat";
            r_ChatLogic = i_ChatLogic;
            SendMessageCommand = new Command(async () => await sendMessageClicked());
            r_ChatLogic.HandleMessageReceived((ChatMessage) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    AllMessages += $"{Environment.NewLine}{ChatMessage.Content}";
                });
            });
            r_ChatLogic.HandleLoadChatReceived((ChatMessages) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach(ChatMessage message in ChatMessages)
                    {
                        AllMessages += $"{Environment.NewLine}{message.Content}";
                    }
                });
            });

            Task.Run(() =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                await r_ChatLogic.StartConnection());
            });
        }

        private async Task sendMessageClicked()
        {
            if (IsBusy == true)
            {
                return;
            }
                IsBusy = true;
                await r_ChatLogic.SendMessage(ChatRoomId, "User name: " + Message);
                Message = String.Empty;
                IsBusy = false;
        }
    }
}
