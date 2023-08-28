using BeThere.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeThere.Views.Templates
{
    internal class MessageDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SenderMessageTemplate { get; set; }
        public DataTemplate ReceiverMessageTemplate { get; set; }
        private bool flag = false;

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            //var message = (ChatMessage)item;

            //if (message.Sender == null)
            //    return ReceiverMessageTemplate;

            //return SenderMessageTemplate;
            flag = !flag;
            if (flag)
            {
                
                return ReceiverMessageTemplate;
                
            }
            return SenderMessageTemplate;
        }
    }
}
