using QEX_Chat.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace QEX_Chats.Services
{
    public static class UserService
    {
        public static IServiceCollection AddQEXChat(this IServiceCollection services)
        {
            services.AddSingleton<ChatService>();
            return services;
        }
    }
}
