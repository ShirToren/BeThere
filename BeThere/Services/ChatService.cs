using BeThere.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
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
        private readonly List<string> r_ChatsIdToJoin = new List<string>();
        private readonly List<string> r_ChatsIdToCreate = new List<string>();

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
                LogedInUser.LogedInUserName(),
                message });
        }

        public async Task JoinChatRoom(string chatRoomId)
        { // when opening an old chat, call this method and it will load the chat. leave chat room when exit.

            //if(r_Connection.State == HubConnectionState.Connected)
            //{
                await r_Connection.SendAsync("JoinChatRoom", chatRoomId);
            //}
            //else
            //{
            //    r_ChatsIdToJoin.Add(chatRoomId);  
            //}
        }

        public async Task CreateChatRoom(string chatRoomId)
        {
            //if (r_Connection.State == HubConnectionState.Connected)
            //{
                await r_Connection.SendAsync("CreateChatRoom", chatRoomId);
            //}
            //else
            //{
              //  r_ChatsIdToCreate.Add(chatRoomId);
            //}
        }

        public async Task<ResultUnit<List<ChatMessage>>> TryGetMessagesByChatRoomId(string i_ChatRoomId)
        {
            ResultUnit<List<ChatMessage>> result = new ResultUnit<List<ChatMessage>>();
            string endPointQueryUri = $"api/ChatMessages?ChatRoomId={i_ChatRoomId}";
            HttpResponseMessage response = await GetHttpClient().GetAsync(endPointQueryUri);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<ChatMessage> chatMessages = JsonConvert.DeserializeObject<List<ChatMessage>>(jsonResponse);
                result.ReturnValue = chatMessages;
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                result.IsSuccess = false;
                result.ResultMessage = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException();
            }

            return result;
        }

        public async Task StartConnection()
        {
         
            await r_Connection.StartAsync();
            if (r_ChatsIdToJoin.Count > 0)
            {
                foreach (string id in r_ChatsIdToJoin)
                {
                    await r_Connection.SendAsync("JoinChatRoom", id);
                }
            }
            if (r_ChatsIdToCreate.Count > 0)
            {
                foreach (string id in r_ChatsIdToJoin)
                {
                    await r_Connection.SendAsync("CreateChatRoom", id);
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
        public void HandleUpdateChatRoomsReceived(Action<string> updateChatRoomsReceivedHandler)
        {
            r_Connection.On<string>("UpdateChatRooms", updateChatRoomsReceivedHandler);
        }
    }
}
