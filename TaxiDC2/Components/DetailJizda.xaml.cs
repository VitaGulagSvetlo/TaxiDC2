using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailJizda : ContentPage, IQueryAttributable
	{
		private readonly IBussinessState _bs;
		private readonly IApiProxy _proxy;
		private TripDetailViewModel _viewModel;

		public DetailJizda(TripDetailViewModel vm, IApiProxy proxy, IBussinessState bs)
        {
	        _bs = bs;
	        _proxy = proxy;
	        InitializeComponent();
	        BindingContext = _viewModel = vm;
        }
		
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
	        if (query.ContainsKey("id"))
	        {
		        var idAsString = query["id"]?.ToString();
		        if (Guid.TryParse(idAsString, out var parsedId))
		        {
			        
			        var vm = BindingContext as TripDetailViewModel;
			        vm?.LoadData(parsedId,_proxy);
		        }
	        }
        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");
        }
		
		private async void Sms_OnClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync($"{nameof(SmsSendView)}?IdTrip={_viewModel.IdTrip}&Phone={Uri.EscapeDataString(_viewModel.Customer.PhoneNumber)}");

			if (_viewModel.TripState != TripState.SMS1sended)
			{
				ServiceResult ret = await _proxy.ChangeTripState(_viewModel.IdTrip, TripState.SMS1sended);
				if (ret.State == ResultCode.OK)
					_viewModel.TripState = TripState.SMS1sended;
			}
			else
			{
				ServiceResult ret = await _proxy.ChangeTripState(_viewModel.IdTrip, TripState.SMS2sended);
				if (ret.State == ResultCode.OK)
					_viewModel.TripState = TripState.SMS2sended;
			}
		}

		private async void Call_OnClicked(object sender, EventArgs e)
		{
			await PlacePhoneCall(_viewModel.Customer.PhoneNumber);
			ServiceResult ret = await _proxy.ChangeTripState(_viewModel.IdTrip, TripState.Call);
			if (ret.State == ResultCode.OK)
				_viewModel.TripState = TripState.Call;
		}

		private async void Acc_OnClicked(object sender, EventArgs e)
		{
			if (_bs.DriverId == null)
			{
				await DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			ServiceResult ret = await _proxy.AcceptByDriver(_viewModel.IdTrip, _bs.DriverId.Value);
			if (ret.State == ResultCode.OK)
			{
				_viewModel.TripState = TripState.AcceptedDiver;
				await _viewModel.LoadData(_viewModel.IdTrip,_proxy);
			}
			else
			{
				_viewModel.Message = ret.Message;
				await DisplayAlert("POZOR", ret.Message, "OK");
			}
		}

		/// <summary>
		/// akceptace z WWW
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Con_OnClicked(object sender, EventArgs e)
		{
			if (_bs.DriverId == null)
			{
				await DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			ServiceResult ret = await _proxy.ChangeTripState(_viewModel.IdTrip, TripState.NewOrder);
			if (ret.State == ResultCode.OK)
			{
				try
				{
					string messageText = $"Pøijali jsme vaší objednávku z WWW. Dìkujeme Taxi-Dìèín 777557776 ";
					SmsMessage message = new(messageText, new[] { _viewModel.Customer.PhoneNumber });
					await Sms.ComposeAsync(message);
				}
				catch (FeatureNotSupportedException ex)
				{
					// Sms is not supported on this device.
				}
				catch (Exception ex)
				{
					// ignored
				}

				_viewModel.TripState = TripState.NewOrder;
				await _viewModel.LoadData(_viewModel.IdTrip, _proxy);
			}
			else
			{
				_viewModel.Message = ret.Message;
				await DisplayAlert("POZOR", ret.Message, "OK");
			}
		}

		private async void Rej_OnClicked(object sender, EventArgs e)

		{
			if (_bs.DriverId == null)
			{
				await DisplayAlert("ERROR", "No active driver","OK");
				return;
			}

			ServiceResult ret = await _proxy.RejectTrip(_viewModel.IdTrip, _bs.DriverId.Value);
			if (ret.State == ResultCode.OK)
			{
				_viewModel.TripState = TripState.RejectedByDiver;
				await _viewModel.LoadData(_viewModel.IdTrip, _proxy);
			}
			else
			{
				_viewModel.Message = ret.Message;
				await DisplayAlert("POZOR", ret.Message, "OK");
			}
		}

		private async void Run_OnClicked(object sender, EventArgs e)
		{
			if (_bs.DriverId == null)
			{
				await DisplayAlert("ERROR","No active driver","OK");
				return;
			}

			ServiceResult ret = await _proxy.ChangeTripState(_viewModel.IdTrip, TripState.Running);
			if (ret.State == ResultCode.OK)
			{
				_viewModel.TripState = TripState.Running;
				await _viewModel.LoadData(_viewModel.IdTrip, _proxy);
			}
			else
			{
				_viewModel.Message = ret.Message;
				await DisplayAlert("POZOR", ret.Message, "OK");
			}
		}

		private async void Storno_OnClicked(object sender, EventArgs e)
		{
			ServiceResult ret = await _proxy.ChangeTripState(_viewModel.IdTrip, TripState.Canceled);
			if (ret.State == ResultCode.OK)
			{
				//_viewModel.TripState = TripState.Canceled;
				//BindingContext = _viewModel = Task.Run(async () => await LoadData()).Result;
				await Shell.Current.GoToAsync("..");
			}
			else
			{
				_viewModel.Message = ret.Message;
				await DisplayAlert("POZOR", ret.Message, "OK");
			}
		}

		private async void Complete_OnClicked(object sender, EventArgs e)
		{
			ServiceResult ret = await _proxy.ChangeTripState(_viewModel.IdTrip, TripState.Comleted);
			if (ret.State == ResultCode.OK)
			{
				//_viewModel.TripState = TripState.Comleted;
				//await Shell.Current.GoToAsync($"///{nameof(TripListPage)}");
				await Shell.Current.GoToAsync("..");
			}
		}

		private async Task PlacePhoneCall(string number)
		{
			if (PhoneDialer.Default.IsSupported)
			{
				try
				{
					PhoneDialer.Open(number);
				}
				catch (ArgumentNullException)
				{
					await DisplayAlert("POZOR", "Neznámé tel. èíslo", "OK");
				}
				catch (Exception ex)
				{
					await DisplayAlert("", $"Chyba \n{ex.Message}", "OK");
				}
			}
			else
			{
				await DisplayAlert("", $"Call not supported", "OK");
			}
		}

		private async void Forward_OnClicked(object sender, EventArgs e)
		{
			_viewModel.PickerVisible = true;
		}

		private async void ListViewLoc_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			ListView picker = (ListView)sender;
			if (e.SelectedItem is Driver sel)
			{
				await ForwardTripToDriver(sel);
			}
			_viewModel.PickerVisible = false;
		}

		public async Task ForwardTripToDriver(Driver drv)
		{
			ServiceResult res = await _proxy.ForwardTrip(_viewModel.IdTrip, drv.IdDriver);
			if (res.State == ResultCode.OK)
			{
				await _viewModel.LoadData(_viewModel.IdTrip, _proxy);
			}
			await DisplayAlert("POZOR", res.Message,"OK");
		}

	}
}
