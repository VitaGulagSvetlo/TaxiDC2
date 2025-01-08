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

#if DEBUG
			builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
