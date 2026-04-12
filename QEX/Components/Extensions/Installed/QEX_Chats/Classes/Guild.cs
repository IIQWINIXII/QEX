using QEX_Chat.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Chat.Classes
{
    public class Guild
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = "";

        public List<СhatChannel> TextChannels { get; set; } = new();
        public List<VoiceChannel> VoiceChannels { get; set; } = new();
    }
}
