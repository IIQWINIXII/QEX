using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;
using QEX.Abstractions.Interface;
using QEX.Abstractions.Service;

namespace QEX
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<IExtensionService, ExtensionService>();
            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);
            builder.Services.AddMauiBlazorWebView();

#if DEBUG

            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
