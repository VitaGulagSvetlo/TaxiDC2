using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxiDC2.Interfaces;

namespace TaxiDC2.ViewModels
{
	public partial class TripDetailViewModel : BaseViewModel
	{
		private readonly IBussinessState _bs;
		//public ObservableCollection<Driver> ListRidicu { get; } = new();

		[ObservableProperty] private Trip _trip = new();

		[ObservableProperty] public bool _pickerVisible = false;
		[ObservableProperty] private bool _complete = false;

		public TripDetailViewModel(IDataService dataService, IBussinessState bs) : base(dataService)
		{
			_bs = bs;
		}

		[DependsOn("TripState")] public bool BtnConVisibility => Trip.TripState is (TripState.NewWWW);

		[DependsOn("TripState")]
		public bool BtnAccVisibility => IsOwner && Trip.TripState == TripState.ForwardToDiver ||
		                                Trip.TripState is (TripState.NewOrder or TripState.RejectedByDiver);

		[DependsOn("TripState")]
		public bool BtnCancelVisibility => Trip.TripState is not (TripState.Comleted or TripState.Canceled);

		[DependsOn("TripState", "Driver")]
		public bool BtnRejectVisibility => IsOwner && Trip.TripState == TripState.ForwardToDiver;

		[DependsOn("TripState", "Driver")]
		public bool BtnForwardVisibility => Trip.TripState is (TripState.NewOrder or TripState.RejectedByDiver);

		[DependsOn("TripState", "Driver")]
		public bool BtnSmsVisibility => IsOwner && Trip.TripState is (TripState.AcceptedDiver or TripState.SMS1sended);

		[DependsOn("TripState", "Driver")]
		public bool BtnCallVisibility => IsOwner &&
		                                 Trip.TripState is (TripState.AcceptedDiver or TripState.SMS1sended
			                                 or TripState.SMS2sended);

		[DependsOn("TripState", "Driver")]
		public bool BtnRunningVisibility => IsOwner && Trip.TripState is (TripState.AcceptedDiver or TripState.Call
			or TripState.SMS1sended or TripState.SMS2sended);

		[DependsOn("TripState", "Driver")]
		public bool BtnCompleteVisibility => IsOwner && Trip.TripState is (TripState.AcceptedDiver or TripState.Running
			or TripState.SMS1sended or TripState.SMS2sended or TripState.Call);

		[DependsOn("Memo")] public bool IsMemoNotEmpty => !string.IsNullOrWhiteSpace(Trip.Memo);
		public bool CustomerMemoVisible => !string.IsNullOrWhiteSpace(Trip.Customer?.Memo);

		public Boolean IsOwner => Trip.Driver != null && Trip.Driver.IdDriver == _bs.DriverId;

		/// <summary>
		/// Adresy jsou validni pro hledani trasy
		/// </summary>
		[DependsOn("AddressBoardingIsValid,AddressExitIsValid")]
		public bool IsValidForRoute => Trip.AddressExitIsValid || Trip.AddressBoardingIsValid;

		[DependsOn("DeadLine,Counter1")]
		public int MinToDeadLine
		{
			get
			{
				if (Trip.BoardingTime != null) return (int)(Trip.BoardingTime.Value - DateTime.Now).TotalMinutes;
				else
				{
					return
						Trip.DeadLine.HasValue
							? ((Trip.OrderTime + Trip.DeadLine.Value - DateTime.Now).TotalMinutes > 0
								? (int)(Trip.OrderTime + Trip.DeadLine.Value - DateTime.Now).TotalMinutes
								: 0)
							: 0;

				}
			}
		}

		public bool MinLabelVisible { get; set; }

		[DependsOn("MinToDeadLine")]
		public string MinToDeadLineTxt
		{
			get
			{
				if (MinToDeadLine > 60 * 24)
				{
					MinLabelVisible = false;
					return $"{Trip.BoardingTime:dd.MM. HH:mm}";
				}

				MinLabelVisible = true;
				return MinToDeadLine < 0 ? "0" : MinToDeadLine.ToString();
			}
		}


		[DependsOn("TripState")]
		public Color StateColor
		{
			get
			{
				Color c = (Color)Application.Current.Resources[$"STC_{Trip.TripState.ToString()}"]; //nova neprevzata
				return c;
			}
		}

		[DependsOn("TripState")]
		public Color StateTextColor
		{
			get
			{
				Color c = ContrastColor(StateColor);

				return c;
			}
		}

		Color ContrastColor(Color color)
		{
			int d = 0;

			// Counting the perceptive luminance - human eye favors green color...      
			double luminance = (0.299 * color.Red + 0.587 * color.Green + 0.114 * color.Blue) / 255;

			d = luminance > 0.5 ? 0 : 255;

			return Color.FromRgb(d, d, d);
		}

		[DependsOn("TripState")]
		public FileImageSource StateImage
		{
			get
			{
				FileImageSource i = new();

				switch (Trip.TripState)
				{
					case TripState.NewOrder:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png"); // hvezdicka
						break;

					case TripState.RejectedByDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png"); // hvezdicka
						break;

					case TripState.ForwardToDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png"); // user
						break;

					case TripState.AcceptedDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png"); // user
						break;

					case TripState.Running:
						i = (FileImageSource)ImageSource.FromFile("ico_car_black.png"); // auticko
						break;

					case TripState.SMS1sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms1_black.png"); // smska
						break;

					case TripState.SMS2sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms2_black.png"); // smska
						break;

					case TripState.Call:
						i = (FileImageSource)ImageSource.FromFile("ico_phone_black.png"); // sluchatko
						break;

					case TripState.Comleted:
						i = (FileImageSource)ImageSource.FromFile("ico_thumbsup_black.png"); // palec nahoru
						break;

					case TripState.Canceled:
						i = (FileImageSource)ImageSource.FromFile("ico_cross_black.png"); // krizek
						break;

					case TripState.NewWWW:
						i = (FileImageSource)ImageSource.FromFile("ico_globe_black.png"); // globus
						break;

					default:
						break;
				}

				return i;
			}
		}

		[DependsOn("DeadLine,Counter1")]
		public Color TimeColor =>
			Trip.TripState == TripState.Canceled || Trip.TripState == TripState.Comleted
				? (Color)Application.Current.Resources["PozadiTmava"]
				: MinToDeadLine < 2
					? (Color)Application.Current.Resources["Cervena"]
					: MinToDeadLine < 5
						? (Color)Application.Current.Resources["Oranzova"]
						: MinToDeadLine < 60
							? (Color)Application.Current.Resources["Zelena"]
							: (Color)Application.Current.Resources["Zluta2"];

		[DependsOn("TripState")]
		public bool TimeVisible => Trip.TripState != TripState.Canceled && Trip.TripState != TripState.Comleted;

		public void RefreshTime()
		{
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinToDeadLine)));
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinToDeadLineTxt)));
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeColor)));
		}

		[RelayCommand]
		private async Task PhoneNumberTapped()
		{
			if (Trip.Customer != null)
				await Shell.Current.GoToAsync($"{nameof(DetailZakaznik)}?id={Trip.Customer.IdCustomer}");
		}

		public async Task LoadData(Guid id)
		{
			var trip = await DataService.GetTripByIdAsync(id);
			if (trip != null)
			{
				Trip.AddressBoarding = trip.AddressBoarding;
				Trip.AddressBoardingIsValid = trip.AddressBoardingIsValid;
				Trip.AddressBoardingLocX = trip.AddressBoardingLocX;
				Trip.AddressBoardingLocY = trip.AddressBoardingLocY;
				Trip.AddressExit = trip.AddressExit;
				Trip.AddressExitIsValid = trip.AddressExitIsValid;
				Trip.AddressExitLocX = trip.AddressExitLocX;
				Trip.AddressExitLocY = trip.AddressExitLocY;
				Trip.BoardingTime = trip.BoardingTime;
				Trip.Complete = trip.Complete;
				Trip.Customer = trip.Customer;
				Trip.DeadLine = trip.DeadLine;
				Trip.Driver = trip.Driver;
				Trip.ExitTime = trip.ExitTime;
				Trip.IdTrip = trip.IdTrip;
				Trip.Memo = trip.Memo;
				Trip.OrderTime = trip.OrderTime;
				Trip.TripState = trip.TripState;
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
					await Shell.Current.DisplayAlert("POZOR", "Neznámé tel. číslo", "OK");
				}
				catch (Exception ex)
				{
					await Shell.Current.DisplayAlert("", $"Chyba \n{ex.Message}", "OK");
				}
			}
			else
			{
				await Shell.Current.DisplayAlert("", $"Call not supported", "OK");
			}
		}

		[RelayCommand]
		private async Task SmsOpen()
		{
			await Shell.Current.GoToAsync(
				$"{nameof(SmsSendView)}?IdTrip={Trip.IdTrip}&Phone={Uri.EscapeDataString(Trip.Customer.PhoneNumber)}");

			if (Trip.TripState != TripState.SMS1sended)
			{
				var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.SMS1sended);
				if (ret)
					Trip.TripState = TripState.SMS1sended;
			}
			else
			{
				var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.SMS2sended);
				if (ret)
					Trip.TripState = TripState.SMS2sended;
			}
		}

		[RelayCommand]
		private async Task Call()
		{
			await PlacePhoneCall(Trip.Customer.PhoneNumber);
			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Call);
			if (ret)
				Trip.TripState = TripState.Call;
		}

		[RelayCommand]
		private async Task Acc()
		{
			if (_bs.DriverId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.AcceptTripByDriverAsync(Trip.IdTrip, _bs.DriverId.Value);
			if (ret)
			{
				Trip.TripState = TripState.AcceptedDiver;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla akceptována", "OK");
			}
		}

		[RelayCommand]
		private async Task Con()
		{
			if (_bs.DriverId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.NewOrder);
			if (ret)
			{
				try
				{
					string messageText = $"Přijali jsme vaší objednávku z WWW. Děkujeme Taxi-Děčín 777557776 ";
					SmsMessage message = new(messageText, new[] { Trip.Customer.PhoneNumber });
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

				Trip.TripState = TripState.NewOrder;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda z WWW nebyla akceptována", "OK");
			}
		}

		[RelayCommand]
		private async Task Rej()

		{
			if (_bs.DriverId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.RejectTripAsync(Trip.IdTrip, _bs.DriverId.Value);
			if (ret)
			{
				Trip.TripState = TripState.RejectedByDiver;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla odmítnuta", "OK");
			}
		}

		[RelayCommand]
		private async Task Run()
		{
			if (_bs.DriverId == null)
			{
				await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
				return;
			}

			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Running);
			if (ret)
			{
				Trip.TripState = TripState.Running;
				await LoadData(Trip.IdTrip);
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla spuštěna", "OK");
			}
		}

		[RelayCommand]
		private async Task Storno()
		{
			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Canceled);
			if (ret)
			{
				//Trip.TripState = TripState.Canceled;
				//BindingContext = _viewModel = Task.Run(async () => await LoadData()).Result;
				await Shell.Current.GoToAsync("..");
			}
			else
			{
				await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla stornována", "OK");
			}
		}

		[RelayCommand]
		private async Task Completed()
		{
			var ret = await DataService.ChangeTripStateAsync(Trip.IdTrip, TripState.Comleted);
			if (ret)
			{
				//Trip.TripState = TripState.Comleted;
				//await Shell.Current.GoToAsync($"///{nameof(TripListPage)}");
				await Shell.Current.GoToAsync("..");
			}
		}

		private async Task Forward()
		{
			PickerVisible = true;
		}

		public async Task ForwardTripToDriver(Driver drv)
		{
			var res = await DataService.ForwardTripAsync(Trip.IdTrip, drv.IdDriver);
			if (res)
			{
				await LoadData(Trip.IdTrip);
				await Shell.Current.DisplayAlert("POZOR", $"Jízda předána {drv.FullName}", "OK");
			}
		}

		[RelayCommand]
		private async Task CustomerEdit()
		{
			if (Trip.Customer != null)
				await Shell.Current.GoToAsync($"{nameof(DetailZakaznik)}?id={Trip.Customer.IdCustomer}");
		}
	}
}