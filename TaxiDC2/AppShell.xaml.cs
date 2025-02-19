using TaxiDC2.Components.Login;

namespace TaxiDC2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

			RegisterAllRoutes();
        }

		/// <summary>
		/// Registruje routy pro navigaci
		/// </summary>
        private void RegisterAllRoutes()
        {
	        Routing.RegisterRoute(nameof(DetailAuto), typeof(DetailAuto));
	        Routing.RegisterRoute(nameof(DetailJizda), typeof(DetailJizda));
	        Routing.RegisterRoute(nameof(DetailRidic), typeof(DetailRidic));
	        Routing.RegisterRoute(nameof(DetailZakaznik), typeof(DetailZakaznik));
	        Routing.RegisterRoute(nameof(NovaJizda), typeof(NovaJizda));
	        Routing.RegisterRoute(nameof(SeznamAut), typeof(SeznamAut));
	        Routing.RegisterRoute(nameof(SeznamRidicu), typeof(SeznamRidicu));
	        Routing.RegisterRoute(nameof(SeznamZakazniku), typeof(SeznamZakazniku));
				        Routing.RegisterRoute(nameof(SeznamJizd), typeof(SeznamJizd));
				        Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
				        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
			//	        Routing.RegisterRoute(nameof(), typeof());
			Routing.RegisterRoute(nameof(TripAlert), typeof(TripAlert));
			Routing.RegisterRoute(nameof(SmsSendView), typeof(SmsSendView));
			Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));

		}

	}
}
