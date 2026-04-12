using QEX_Chat.Classes;
using QEX_Lib.ClientDB.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace QEX.Components.Extensions.Installed.QEX_Chats.Classes
{
    public class DirectChat
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public User User { get; set; } = new(); 
        public ObservableCollection<ChatMessage> Messages { get; set; } = new();
    }
}
