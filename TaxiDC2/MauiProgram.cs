using IdentityModel.OidcClient;
using MauiApp1;
using Microsoft.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using TaxiDC2.Services;

namespace TaxiDC2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddLogging();
            builder.Services.AddScoped<IDataService, DataService>();

            // setup OidcClient
            builder.Services.AddSingleton(new OidcClient(new()
            {
	            Authority = "https://demo.duendesoftware.com",

	            ClientId = "interactive.public",
	            Scope = "openid profile api",
	            RedirectUri = "taxidc://callback",
                LoadProfile = true,
                Browser = new MauiAuthenticationBrowser()
            }));

            // add main page
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<SeznamAut>();

			// add services
			builder.Services.AddSingleton<IIdentityHelper,IdentityHelper>();
            builder.Services.AddSingleton<IBussinessState,BussinessState>();
            builder.Services.AddSingleton<IApiProxy, ApiProxy>();

#if DEBUG
			builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
