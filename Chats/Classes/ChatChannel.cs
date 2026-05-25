using System;
using System.Collections.Generic;
using System.Text;

namespace Chats.Classes
{
    public class СhatChannel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
        public List<ChatMessage> Messages { get; set; } = new();
    }
}
