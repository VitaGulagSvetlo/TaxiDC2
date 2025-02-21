using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Auth;
using Plugin.Maui.Biometric;
using TaxiDC2.Components;
using TaxiDC2.Components.Login;

namespace TaxiDC2.ViewModels
{
	public partial class LoadingPageViewModel : BaseViewModel
	{
		private readonly FirebaseAuthClient _authClient;
		private readonly IServiceProvider _serviceProvider;
		private readonly IBiometric _biometricService;

		public LoadingPageViewModel(IDataService dataService, FirebaseAuthClient authClient, IServiceProvider serviceProvider, IBiometric biometricService) : base(dataService)
		{
			_authClient = authClient;
			_serviceProvider = serviceProvider;
			_biometricService = biometricService;
			CheckUser();
		}

		private async void CheckUser()
		{
			if (string.IsNullOrWhiteSpace(_authClient?.User?.Info?.Email))
			{
				if (DeviceInfo.Platform == DevicePlatform.WinUI)
				{
					Shell.Current.Dispatcher.Dispatch(async () =>
					{
						await Shell.Current.GoToAsync(nameof(SignInPage));
					});
				}
				else
				{
					await Shell.Current.GoToAsync(nameof(SignInPage));
				}
			}
			else
			{
				//todo: debug
				Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
				await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
				return;
				//todo: debug

				var biometricStatus = await _biometricService.GetAuthenticationStatusAsync();
				// Handle biometricStatus based on the application's logic

				if (biometricStatus != BiometricHwStatus.Success)
				{
					await Shell.Current.DisplayAlert("Error", "biometricStatus is false", "Close");
					
					return;
				}

				var authenticationRequest = new AuthenticationRequest
				{
					AllowPasswordAuth = true, // A chance to fallback to password auth
					Title = "Authenticate", // On iOS only the title is relevant, everything else is unused. 
					Subtitle = "Please authenticate using your biometric data",
					//NegativeText = "Use Password", // if AllowPasswordAuth is set to true don't use this it will throw an exception on Android
					Description = "Biometric authentication is required for access",
					AuthStrength = AuthenticatorStrength.Strong // Only relevant on Android
				};

				//tady je to v dispatcheru, protoze jinak to bezelo v jinem tasku nez UI
				//a vyhazovalo to nesmyslne chyby v Android java kodu
				// rozchozeno nahodou a nevim co to presne dela :-)
				Shell.Current.Dispatcher.Dispatch(async () =>
				{
					var authenticationResponse = await _biometricService.AuthenticateAsync(authenticationRequest, CancellationToken.None);
					if (authenticationResponse.Status != BiometricResponseStatus.Success)
					{
						await Shell.Current.DisplayAlert("Error", authenticationResponse.ErrorMsg, "Close");
						return;
					}
					else
					{
						Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
						await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
					}
				});

			}
		}
	}
}
