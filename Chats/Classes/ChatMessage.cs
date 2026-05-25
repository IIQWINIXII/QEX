using QEX_Lib.ClientDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chats.Classes
{
    public class ChatMessage
    {
        public User? Sender { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
