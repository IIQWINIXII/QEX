using Microsoft.Extensions.Logging;
using QEX.Abstractions.Interface;
using QEX.Abstractions.Service;
using QEX_Chat.Services;

namespace QEX
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<IExtensionService, ExtensionService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<ChatService, ChatService>();
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
