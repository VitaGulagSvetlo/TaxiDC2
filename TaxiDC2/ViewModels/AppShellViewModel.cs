using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using TaxiDC2.Components.Login;

namespace TaxiDC2.ViewModels
{
	public partial class AppShellViewModel:BaseViewModel
	{
		private readonly FirebaseAuthClient _authClient;

		public AppShellViewModel(FirebaseAuthClient authClient)
		{
			_authClient = authClient;
		}

		public bool IsSigned => !string.IsNullOrWhiteSpace(_authClient?.User?.Info?.Email);
		public bool IsNotSigned => !IsSigned;

		[RelayCommand]
		async void Logout()
		{
			bool ansver = await Shell.Current.DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
			if(ansver)
			{
				_authClient.SignOut();
				await Shell.Current.GoToAsync($"{nameof(SignInPage)}");
			}
		}
	}
}
