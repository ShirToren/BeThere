using BeThere.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.Services
{
    public class ChatService : BaseService
    {
        private readonly HubConnection r_Connection;
        private readonly List<string> r_ChatsId = new List<string>();
        
        public ChatService()
        {
            r_Connection = new HubConnectionBuilder()
            .WithUrl($"{s_BaseUrl}/chat")
            .Build();
        }

        public async Task SendMessage(string chatRoomId, string message)
        {
            await r_Connection.InvokeCoreAsync("SendMessage", args: new[] {
                chatRoomId, 
                "User",
                message });
        }

        public async Task JoinChatRoom(string chatRoomId)
        {
            if(r_Connection.State == HubConnectionState.Connected)
            {
                await r_Connection.SendAsync("JoinChatRoom", chatRoomId);
            }
            else
            {
                r_ChatsId.Add(chatRoomId);  
            }
        }

        public async Task StartConnection()
        {
         
            await r_Connection.StartAsync();
            if (r_ChatsId.Count > 0)
            {
                foreach (string id in r_ChatsId)
                {
                    await r_Connection.SendAsync("JoinChatRoom", id);
                }
            }
        }

        public void HandleMessageReceived(Action<ChatMessage> messageReceivedHandler) {
            r_Connection.On<ChatMessage>("MessageReceived", messageReceivedHandler);
        }
        public void HandleLoadChatReceived(Action<List<ChatMessage>> loadChatReceivedHandler)
        {
            r_Connection.On<List<ChatMessage>>("LoadChatHistory", loadChatReceivedHandler);
        }
    }
}
