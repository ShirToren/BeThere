using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.Models
{
    public class UserAnswer
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public string QuestionId { get; set; } 
        public string ChatRoomId { get; set; }


        public UserAnswer(string i_UserName, string i_Text, string i_QuestionId, string i_ChatRoomId)
        {
            Username = i_UserName;
            Text = i_Text;
            QuestionId = i_QuestionId;
            ChatRoomId = i_ChatRoomId;
        }
    }
}
