using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string ChatRoomId { get; set; }
        public ChatMessage(string sender, string content, DateTime time, string id)
        {
            Sender = sender;
            Content = content;
            Timestamp = time;
            ChatRoomId = id;
        }
        public ChatMessage()
        {
                
        }
    }
}
