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
using BeThere.Views;
using Mopups.Interfaces;
using System.Collections.ObjectModel;

namespace BeThere.ViewModels.ChatViewModel
{
    [QueryProperty(nameof(ChatRoomId), "ChatRoomId")]
    public partial class ChatViewModel : BaseViewModel
    {
        private readonly ChatService r_ChatLogic;
        public Command SendMessageCommand { get; }
        public ObservableCollection<ChatMessage> CurrentChatMessages => SharedDataSource.CurrentChatMessages;


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
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    //AllMessages += $"{Environment.NewLine}{ChatMessage.Content}";
                    SharedDataSource.CurrentChatMessages.Add(ChatMessage);
                });
            });
            r_ChatLogic.HandleLoadChatReceived((ChatMessages) =>
            {
                foreach(ChatMessage chatMessage in ChatMessages)
                {
                    SharedDataSource.CurrentChatMessages.Add(chatMessage);
                }

/*                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach(ChatMessage message in ChatMessages)
                    {
                        AllMessages += $"{Environment.NewLine}{message.Content}";
                    }
                });*/
            });

            Task.Run(() =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                await r_ChatLogic.StartConnection());
            });
        }

        public void clearMessages()
        {
            SharedDataSource.CurrentChatMessages.Clear();
        }

        private async Task sendMessageClicked()
        {
            if (IsBusy == true)
            {
                return;
            }
                IsBusy = true;
                await r_ChatLogic.SendMessage(ChatRoomId, Message);
                Message = String.Empty;
                IsBusy = false;
        }
    }
}
