using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TaxiDC2.Components;

namespace TaxiDC2.ViewModels
{
	public partial class SignInViewModel : BaseViewModel
	{
		private readonly FirebaseAuthClient _authClient;
		private readonly IDataService _dataService;
		private readonly IBussinessState _bs;
		public string Email { get; set; } = "peno@penodc.com";
		public string Password { get; set; } = "penox22";
		public string Message { get; set; }
		public bool IsMessageVisible => !string.IsNullOrWhiteSpace(Message);

		public bool ServerOK
		{
			get
			{
				bool r = Task.Run(async ()=> await _dataService.PingAsync()).Result;
				return r;
			}
		}

		/// <inheritdoc/>
		public SignInViewModel(IDataService dataService, FirebaseAuthClient authClient,IBussinessState bs) : base(dataService)
		{
			_authClient = authClient;
			_dataService = dataService;
			_bs = bs;
		}

		[RelayCommand]
		private async Task SignIn()
		{
			Message = String.Empty;

			if(string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
			{
				Message = "Email and password are required !";
				return;
			}

			UserCredential result = await _authClient.SignInWithEmailAndPasswordAsync(Email,Password);
			if (result.User!=null)
			{
				await LoadDriver();
				Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
				Shell.Current.GoToAsync($"///{nameof(MainPage)}",false);
			}
		}

		private async Task LoadDriver()
		{
			Driver[] drl = await _dataService.GetDriversAsync(true);
			Driver driver = drl.FirstOrDefault(f => f.MobileDeviceHash == _authClient.User.Uid);
			if (driver != null)
			{
				_bs.Driver = driver;
			}
			else
			{
				_bs.Driver = null;
			}
		}

	}
}
