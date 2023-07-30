using Client.Services.Listeners;
using Blazored.LocalStorage;
using MudBlazor.Services;
using Client.Services;
using Client.Hubs.Agents;

namespace Client
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

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddMudServices();

            builder.Services.AddSingleton<ListenerService>();
			builder.Services.AddSingleton<ListenerTypeService>();
			builder.Services.AddSingleton<ApiService>();
			builder.Services.AddSingleton<AmberHub>();

            builder.Services.AddBlazoredLocalStorage();

          
            return builder.Build();
        }
    }
}