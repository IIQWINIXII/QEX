using Chats.Services;
using QEX_Lib.QEX_API.Abtractions.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Module
{
    internal class Module
    {
        public static void Register(IDynamicServiceRegistry registry)
        {
            registry.RegisterScoped<IChatService, ChatService>();
        }
    }
}
