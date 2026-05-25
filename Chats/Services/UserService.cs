using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chats.Services
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
