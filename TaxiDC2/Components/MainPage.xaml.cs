
using TaxiDC2.Interfaces;

namespace TaxiDC2
{
	public partial class MainPage : ContentPage
	{
		private readonly IBussinessState _bussinessState;
		private readonly IApiProxy _proxy;
		private string? _currentAccessToken;

		public MainPage(
			IBussinessState bussinessState,
			IApiProxy proxy)
		{
			InitializeComponent();

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
