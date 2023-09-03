using BeThere.Models;
using BeThere.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.ViewModels.ChatViewModel
{
    public class ChatHistoryViewModel : BaseViewModel
    {
        private ObservableCollection<ChatMessage> m_AvailableChats;
        private ChatService m_ChatService;
        private HashSet<string> m_AvailableChatRooms;


        public ChatHistoryViewModel(ChatService i_ChatService)
        {
            m_AvailableChats = new ObservableCollection<ChatMessage>();
            m_ChatService = i_ChatService;
            m_AvailableChatRooms = new HashSet<string>();


            m_ChatService.HandleUpdateChatRoomsReceived(async (chatRoom) =>
            {
                    if (!m_AvailableChatRooms.Contains(chatRoom))
                    {
                        m_AvailableChatRooms.Add(chatRoom);
                        ResultUnit<List<ChatMessage>> response = await m_ChatService.TryGetMessagesByChatRoomId(chatRoom);
                        if(response.ReturnValue?.Count > 0)
                        {
                            m_AvailableChats.Add(response.ReturnValue[response.ReturnValue.Count - 1]);
                        }
                        else
                        {
                            m_AvailableChats.Add(new ChatMessage("shir", "hello", DateTime.Now, "123"));
                        }
                    }
                
            });
        }
        public ObservableCollection<ChatMessage> AvailableChats { get { return m_AvailableChats; } }
    }
}
