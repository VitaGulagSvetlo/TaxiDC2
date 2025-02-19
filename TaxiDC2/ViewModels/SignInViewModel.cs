using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TaxiDC2.Components;

namespace TaxiDC2.ViewModels
{
	public partial class SignInViewModel : BaseViewModel
	{
		private readonly FirebaseAuthClient _authClient;
		public string Email { get; set; } = "peno@penodc.com";
		public string Password { get; set; } = "penox22";
		public string Message { get; set; }
		//public ICommand SignInCommand { get; }

		/// <inheritdoc/>
		public SignInViewModel(FirebaseAuthClient authClient)
		{
			_authClient = authClient;
			//SignInCommand = new Command(async () => await SignIn());
		}

		[RelayCommand]
		private async Task SignIn()
		{
			var result = await _authClient.SignInWithEmailAndPasswordAsync(Email,Password);
			if (result.User!=null)
			{
				Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
				Shell.Current.GoToAsync($"///{nameof(MainPage)}",true);
			}
		}

		[RelayCommand]
		private async Task SignInGoogle()
		{
			var result = await _authClient. SignInWithRedirectAsync( FirebaseProviderType.Google,RedirectDelegate);
			if (result.User != null)
			{
			}
		}

		private Task<string> RedirectDelegate(string uri)
		{
			return Task.FromResult("dsd");
		}
	}
}
