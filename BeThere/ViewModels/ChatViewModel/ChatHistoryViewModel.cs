using BeThere.Models;
using BeThere.Services;
using BeThere.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BeThere.ViewModels.ChatViewModel
{
    public class ChatHistoryViewModel : BaseViewModel
    {
        private ObservableCollection<ChatMessage> m_AvailableChats => SharedDataSource.AvailableChats;
        private ChatService m_ChatService;
        private HashSet<string> m_AvailableChatRooms => SharedDataSource.AvailableChatRooms;
        public ICommand ChatCommand { get; private set; }

        public ChatHistoryViewModel(ChatService i_ChatService)
        {
            //m_AvailableChats = new ObservableCollection<ChatMessage>();
            m_ChatService = i_ChatService;
            //m_AvailableChatRooms = new HashSet<string>();
            ChatCommand = new Command<ChatMessage>(chatClicked);
           //removePreferences();

            m_ChatService.HandleUpdateChatRoomsReceived(async (chatRoom) =>
            {
                    if (!m_AvailableChatRooms.Contains(chatRoom))
                    {
                        ResultUnit<List<ChatMessage>> response = await m_ChatService.TryGetMessagesByChatRoomId(chatRoom);
                        if(response.ReturnValue?.Count > 0)
                        {
                            m_AvailableChats.Insert(0, response.ReturnValue[response.ReturnValue.Count - 1]);
                            m_AvailableChatRooms.Add(chatRoom);
                            updatePreferences();
                        }
                    }
            });

            m_ChatService.HandleMessageReceived((ChatMessage) =>
            {
                ChatMessage messageToChange = null;
                foreach (ChatMessage message in m_AvailableChats)
                {
                    if (message.ChatRoomId.Equals(ChatMessage.ChatRoomId))
                    {
                        messageToChange = message;
                    }
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SharedDataSource.CurrentChatMessages.Add(ChatMessage);
                    if(messageToChange != null)
                    {
                        m_AvailableChats.Remove(messageToChange);
                        m_AvailableChats.Insert(0, ChatMessage);
                        updatePreferences();
                    }
                    else
                    {
                        m_AvailableChatRooms.Add(ChatMessage.ChatRoomId);
                        m_AvailableChats.Insert(0, ChatMessage);
                        updatePreferences();
                    }
                });
            });
        }
        public ObservableCollection<ChatMessage> AvailableChats { get { return m_AvailableChats; }  }
        public HashSet<string> AvailableChatRooms { get { return m_AvailableChatRooms; } } 
        private void updatePreferences()
        {
            string serializedMessagesList = JsonSerializer.Serialize(AvailableChats);
            Preferences.Set("MessagesList", serializedMessagesList);

            string serializedRoomSet = JsonSerializer.Serialize(AvailableChatRooms);
            Preferences.Set("ChatRoomsList", serializedRoomSet);
        }

        public async void LoadChats()
        {
            var result = await m_ChatService.TryGetUserRooms();
            if(result.ReturnValue.Count > 0)
            {
                foreach (string room in result.ReturnValue)
                {
                    m_AvailableChatRooms.Add(room);
                    var messageResponse = await m_ChatService.TryGetLastMessageByChatRoomId(room);
                    if(messageResponse.ReturnValue != null)
                    {
                        m_AvailableChats.Add(messageResponse.ReturnValue);
                    }
                }
            }
        }

        private void removePreferences()
        {
            Preferences.Remove("MessagesList");
            Preferences.Remove("ChatRoomsList");
        }

        private async void chatClicked(ChatMessage item)
        {
            if (IsBusy == true)
            {
                return;
            }

            try
            {
                await m_ChatService.JoinChatRoom(item.ChatRoomId);
                await GoToChatPage(item.ChatRoomId);

                IsBusy = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("hey");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task GoToChatPage(string i_ChatRoomId)
        {
            var navigationParameter = new Dictionary<string, object>
            {
                ["ChatRoomId"] = i_ChatRoomId
            };
            await Shell.Current.GoToAsync($"{nameof(ChatPage)}", navigationParameter);
        }
    }
}
