using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Logging;
using QEX.Abstractions.Interface;
using QEX.Abstractions.Service;
using QEX.Components.Extensions.Classes;
using QEX_Lib.QEX_API.Abtractions.Interface;
using QEX_Lib.QEX_API.Abtractions.Service;

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
            builder.Services.AddSingleton<DynamicServiceProvider>();
            builder.Services.AddSingleton<IDynamicServiceRegistry>(sp => sp.GetRequiredService<DynamicServiceProvider>());
            builder.Services.AddSingleton<IPluginServiceProvider>(sp => sp.GetRequiredService<DynamicServiceProvider>());

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            var mauiProvider = app.Services;

            // создаём динамический контейнер поверх MAUI DI
            var dynamicProvider = new DynamicServiceProvider(mauiProvider);

            // регистрируем его как шлюз и как реестр
            // ВАЖНО: регистрируем в уже построенном провайдере через фабрику
            // а не через builder.Services (они уже read-only)
            var servicesField = typeof(ServiceProvider)
                .GetField("_engine", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            // этот хак лучше вообще не трогать — поэтому проще:
            // просто передаём dynamicProvider туда, где он нужен (ExtensionLoader, корневые компоненты и т.п.)

            // Проще и чище: регистрируем его вручную в корневом компоненте через параметр/сервис

            var extensionService = mauiProvider.GetRequiredService<IExtensionService>();
            var loader = new ExtensionLoader(extensionService, dynamicProvider);

            return app;
        }


    }
}
