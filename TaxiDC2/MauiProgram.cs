using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Auth.Repository;
using IdentityModel.OidcClient;
using MauiApp1;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Biometric;
using Syncfusion.Maui.Core.Hosting;
using TaxiDC2.Components;
using TaxiDC2.Components.Login;
using TaxiDC2.Interfaces;
using TaxiDC2.ViewModels;

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
	            .ConfigureFonts(FAs =>
	            {
		            FAs.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
		            FAs.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");

		            FAs.AddFont("FA6-Duotone-Light-300.otf", "FADuotoneLight");
		            FAs.AddFont("FA6-Duotone-Regular-400.otf", "FADuotoneRegular");
		            FAs.AddFont("FA6-Duotone-Solid-900.otf", "FADuotoneSolid");
		            FAs.AddFont("FA6-Duotone-Thin-100.otf", "FADuotoneThin");
		            FAs.AddFont("FA6-Pro-Light-300.otf", "FAProLight");
		            //FAs.AddFont("FA6-Pro-Regular-400.otf", "FAProRegular");
		            FAs.AddFont("FA6-Pro-Regular-400.otf", "FAPro");
		            FAs.AddFont("FA6-Pro-Solid-900.otf", "FAProSolid");
		            FAs.AddFont("FA6-Pro-Thin-100.otf", "FAProThin");
		            FAs.AddFont("FA6-SharpDuotone-Light-300.otf", "FASharpDuoLight");
		            FAs.AddFont("FA6-SharpDuotone-Regular-400.otf", "FASharpDuoRegular");
		            FAs.AddFont("FA6-SharpDuotone-Solid-900.otf", "FASharpDuoSolid");
		            FAs.AddFont("FA6-SharpDuotone-Thin-100.otf", "FASharpDuoThin");
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

			builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
				{
				ApiKey = "AIzaSyBdd4AMgyptsYcOC5hhbDuQIiblzwpPfOc",
				AuthDomain = "taxidc2-375cf.firebaseapp.com",
				Providers = [new EmailProvider(),new GoogleProvider()],
				UserRepository = new FileUserRepository("Taxi2")
			}
				
				));

			// add main page
			builder.Services.AddTransient<AboutPage>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<DetailAuto>();
            builder.Services.AddTransient<DetailJizda>();
            builder.Services.AddTransient<DetailRidic>();
			builder.Services.AddTransient<DetailZakaznik>();
			builder.Services.AddTransient<NovaJizda>();
			builder.Services.AddTransient<SeznamAut>();
			builder.Services.AddTransient<SeznamJizd>();
			builder.Services.AddTransient<SeznamRidicu>();
			builder.Services.AddTransient<SeznamZakazniku>();
			builder.Services.AddTransient<SmsSendView>();
			builder.Services.AddTransient<TripNewViewModel>();

			//vm
			builder.Services.AddTransient<ConfigViewModel>();
			builder.Services.AddTransient<TripDetailViewModel>();
			builder.Services.AddTransient<DriverNewViewModel>(); 
			builder.Services.AddTransient<CarDetailViewModel>();
			builder.Services.AddTransient<CustomerDetailViewModel>();
			builder.Services.AddTransient<SmsViewModel>();


			builder.Services.AddTransient<SignInViewModel>();
			builder.Services.AddTransient<SignInPage>();

			builder.Services.AddSingleton<LoadingPageViewModel>();
			builder.Services.AddSingleton<LoadingPage>();

			builder.Services.AddSingleton<AppShellViewModel>();
			builder.Services.AddSingleton<AppShell>();

			// add services
			builder.Services.AddSingleton<IIdentityHelper,IdentityHelper>();
            builder.Services.AddSingleton<IBussinessState,BussinessState>();
            builder.Services.AddSingleton<IApiProxy, ApiProxy>();

            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);

#if ANDROID
	        builder.Services.AddSingleton<ICallLogService, TaxiDC2.Platforms.Android.CallLogService >();
#elif IOS
            builder.Services.AddSingleton<ICallLogService, TaxiDC2.Platforms.iOS.CallLogService >();
#endif

#if DEBUG
			builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
