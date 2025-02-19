using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using TaxiDC2.Components;
using TaxiDC2.Components.Login;

namespace TaxiDC2.ViewModels
{
	public partial class LoadingPageViewModel: BaseViewModel
	{
		private readonly FirebaseAuthClient _authClient;
		private readonly IServiceProvider _serviceProvider;

		public LoadingPageViewModel(FirebaseAuthClient authClient,IServiceProvider serviceProvider)
		{
			_authClient = authClient;
			_serviceProvider = serviceProvider;
			CheckUser();
		}

		private async void  CheckUser()
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
				Shell.Current.FlyoutHeader = new FlyoutHeaderControl(_authClient);
				//App.Current.MainPage = _serviceProvider.GetRequiredService<MainPage>();
				await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
			}
		}
	}
}
