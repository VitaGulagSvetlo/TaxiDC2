using IdentityModel.OidcClient;

namespace TaxiDC2
{
	public partial class MainPage : ContentPage
	{
		private readonly OidcClient _client = default!;
		private readonly IIdentityHelper _identityHelper;
		private readonly IBussinessState _bussinessState;
		private readonly IApiProxy _proxy;
		private string? _currentAccessToken;

		public MainPage(
			OidcClient client,
			IIdentityHelper identityHelper,
			IBussinessState bussinessState,
			IApiProxy proxy)
		{
			InitializeComponent();

			_client = client;
			_identityHelper = identityHelper;
			_bussinessState = bussinessState;
			_proxy = proxy;
		}

		private void OnNewClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(NovaJizda)}");
		}

		private void OnListClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");
		}

		private void OnSetClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(AboutPage)}");
		}


	}

}
