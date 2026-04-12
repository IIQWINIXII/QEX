using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Chat.Classes
{
    public class VoiceChannel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";
    }
}
