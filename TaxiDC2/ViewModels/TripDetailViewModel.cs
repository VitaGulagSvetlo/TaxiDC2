using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TaxiDC2.ViewModels
{
	public class TripDetailViewModel : INotifyPropertyChanged
	{
		private readonly IApiProxy _proxy;
		public event PropertyChangedEventHandler PropertyChanged;

		private readonly IBussinessState _bs = DependencyService.Get<IBussinessState>();

		private ObservableCollection<Driver> _listRidicu = new();

		public ObservableCollection<Driver> ListRidicu => _listRidicu;

		public TripDetailViewModel()
		{
			IsAdmin = true /*_bs.IsAdmin*/;
			PhoneNumberTapped = new Command(PerformPhoneNumberTapped);
		}

		internal static TripDetailViewModel FromTrip(Trip data)
		{
			return new TripDetailViewModel()
			{
				AddressBoarding = data.AddressBoarding,
				AddressBoardingIsValid = data.AddressBoardingIsValid,
				AddressBoardingLocX = data.AddressBoardingLocX,
				AddressBoardingLocY = data.AddressBoardingLocY,
				AddressExit = data.AddressExit,
				AddressExitIsValid = data.AddressExitIsValid,
				AddressExitLocX = data.AddressExitLocX,
				AddressExitLocY = data.AddressExitLocY,
				BoardingTime = data.BoardingTime,
				Complete = data.Complete,
				Customer = data.Customer,
				DeadLine = data.DeadLine,
				Driver = data.Driver,
				ExitTime = data.ExitTime,
				IdTrip = data.IdTrip,
				Memo = data.Memo,
				OrderTime = data.OrderTime,
				TripState = data.TripState
			};
		}

		public string AddressBoarding { get; set; } = string.Empty;
		public string AddressExit { get; set; } = string.Empty;

		public string Title { get; set; } = "Detail jízdy";

		public DateTime? BoardingTime { get; set; }

		[DependsOn("TripState")]
		public bool BtnConVisibility => TripState is (TripState.NewWWW);

		[DependsOn("TripState")]
		public bool BtnAccVisibility => IsOwner && TripState == TripState.ForwardToDiver || TripState is (TripState.NewOrder or TripState.RejectedByDiver);

		[DependsOn("TripState")]
		public bool BtnCancelVisibility => TripState is not (TripState.Comleted or TripState.Canceled);

		[DependsOn("TripState", "Driver")]
		public bool BtnRejectVisibility => IsOwner && TripState == TripState.ForwardToDiver;

		[DependsOn("TripState", "Driver")]
		public bool BtnForwardVisibility => TripState is (TripState.NewOrder or TripState.RejectedByDiver);

		[DependsOn("TripState", "Driver")]
		public bool BtnSmsVisibility => IsOwner && TripState is (TripState.AcceptedDiver or TripState.SMS1sended);

		[DependsOn("TripState", "Driver")]
		public bool BtnCallVisibility => IsOwner && TripState is (TripState.AcceptedDiver or TripState.SMS1sended or TripState.SMS2sended);


		[DependsOn("TripState", "Driver")]
		public bool BtnRunningVisibility => IsOwner && TripState is (TripState.AcceptedDiver or TripState.Call or TripState.SMS1sended or TripState.SMS2sended);

		[DependsOn("TripState", "Driver")]
		public bool BtnCompleteVisibility => IsOwner && TripState is (TripState.AcceptedDiver or TripState.Running or TripState.SMS1sended or TripState.SMS2sended or TripState.Call);

		public bool Complete { get; set; } = false;

		public bool IsAdmin { get; }

		// Musi mit Ignore v konfiguraci AUTOMAPPERU
		public int Counter1 { get; set; }

		public Customer Customer { get; set; } = null;

		public short CustomerVIP => Customer?.VIP2 ?? 0;

		public TimeSpan? DeadLine { get; set; }
		public Driver Driver { get; set; } = null;
		public DateTime? ExitTime { get; set; }
		public Guid IdTrip { get; set; } = Guid.Empty;

		[DependsOn("Memo")]
		public Boolean IsMemoNotEmpty => !string.IsNullOrWhiteSpace(Memo);

		[DependsOn("Memo")]
		public Boolean IsOwner => Driver != null && Driver.IdDriver == _bs.DriverId;

		public bool AddressBoardingIsValid { get; set; }

		public bool AddressExitIsValid { get; set; }

		public string AddressBoardingLocX { get; set; }

		public string AddressBoardingLocY { get; set; }

		public string AddressExitLocX { get; set; }

		public string AddressExitLocY { get; set; }

		/// <summary>
		/// Adresy jsou validni pro hledani trasy
		/// </summary>
		[DependsOn("AddressBoardingIsValid,AddressExitIsValid")]
		public bool IsValidForRoute => AddressExitIsValid || AddressBoardingIsValid;

		[Display(Description = "Poznámka k jízdě")]
		public string Memo { get; set; } = string.Empty;

		[DependsOn("DeadLine,Counter1")]
		public int MinToDeadLine
		{
			get
			{
				if (BoardingTime != null) return (int)(BoardingTime.Value - DateTime.Now).TotalMinutes;
				else
				{
					return
						DeadLine.HasValue
						 ? ((OrderTime + DeadLine.Value - DateTime.Now).TotalMinutes > 0
							 ? (int)(OrderTime + DeadLine.Value - DateTime.Now).TotalMinutes
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
					return $"{BoardingTime:dd.MM. HH:mm}";
				}
				MinLabelVisible = true;
				return MinToDeadLine < 0 ? "0" : MinToDeadLine.ToString();
			}
		}


		public DateTime OrderTime { get; set; } = DateTime.Now;

		[DependsOn("TripState")]
		public Color StateColor
		{
			get
			{
				Color c = (Color)Application.Current.Resources[$"STC_{TripState.ToString()}"];     //nova neprevzata
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

				switch (TripState)
				{
					case TripState.NewOrder:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png");        // hvezdicka
						break;

					case TripState.RejectedByDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_star_black.png");        // hvezdicka
						break;

					case TripState.ForwardToDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png");        // user
						break;

					case TripState.AcceptedDiver:
						i = (FileImageSource)ImageSource.FromFile("ico_user_black.png");        // user
						break;

					case TripState.Running:
						i = (FileImageSource)ImageSource.FromFile("ico_car_black.png");         // auticko
						break;

					case TripState.SMS1sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms1_black.png");         // smska
						break;

					case TripState.SMS2sended:
						i = (FileImageSource)ImageSource.FromFile("ico_sms2_black.png");         // smska
						break;

					case TripState.Call:
						i = (FileImageSource)ImageSource.FromFile("ico_phone_black.png");       // sluchatko
						break;

					case TripState.Comleted:
						i = (FileImageSource)ImageSource.FromFile("ico_thumbsup_black.png");    // palec nahoru
						break;

					case TripState.Canceled:
						i = (FileImageSource)ImageSource.FromFile("ico_cross_black.png");       // krizek
						break;

					case TripState.NewWWW:
						i = (FileImageSource)ImageSource.FromFile("ico_globe_black.png");       // globus
						break;

					default:
						break;
				}

				return i;
			}
		}

		[DependsOn("DeadLine,Counter1")]
		public Color TimeColor =>
			TripState == TripState.Canceled || TripState == TripState.Comleted
				? (Color)Application.Current.Resources["PozadiTmava"]
				: MinToDeadLine < 2
					? (Color)Application.Current.Resources["Cervena"]
					: MinToDeadLine < 5
						? (Color)Application.Current.Resources["Oranzova"]
						: MinToDeadLine < 60
							? (Color)Application.Current.Resources["Zelena"]
							: (Color)Application.Current.Resources["Zluta2"];


		public TripState TripState { get; set; } = TripState.NewOrder;

		public string Message { get; set; }

		[DependsOn("TripState")]
		public bool TimeVisible => TripState != TripState.Canceled && TripState != TripState.Comleted;

		public bool PickerVisible { get; set; } = false;

		public void RefreshTime()
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinToDeadLine)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinToDeadLineTxt)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeColor)));
		}


		public Command<Customer> ItemTapped { get; }

		public Command PhoneNumberTapped { get; }
		

		private async void PerformPhoneNumberTapped()
		{
			await Shell.Current.GoToAsync($"{nameof(DetailZakaznik)}?IdCustomer={Customer.IdCustomer}");
		}

		public async Task LoadData(Guid id, IApiProxy proxy)
		{
			if (proxy == null) throw new ArgumentNullException(nameof(proxy));
			var trip = await proxy.GetTripByIdAsync(id);
			if (trip.State == ResultCode.OK && trip.Data != null)
			{
				AddressBoarding = trip.Data.AddressBoarding;
				AddressBoardingIsValid = trip.Data.AddressBoardingIsValid;
				AddressBoardingLocX = trip.Data.AddressBoardingLocX;
				AddressBoardingLocY = trip.Data.AddressBoardingLocY;
				AddressExit = trip.Data.AddressExit;
				AddressExitIsValid = trip.Data.AddressExitIsValid;
				AddressExitLocX = trip.Data.AddressExitLocX;
				AddressExitLocY = trip.Data.AddressExitLocY;
				BoardingTime = trip.Data.BoardingTime;
				Complete = trip.Data.Complete;
				Customer = trip.Data.Customer;
				DeadLine = trip.Data.DeadLine;
				Driver = trip.Data.Driver;
				ExitTime = trip.Data.ExitTime;
				IdTrip = trip.Data.IdTrip;
				Memo = trip.Data.Memo;
				OrderTime = trip.Data.OrderTime;
				TripState = trip.Data.TripState;
			}
		}

		public async Task<ServiceResult> CancelTrip()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult> AcceptTrip()
		{
			throw new NotImplementedException();
		}
	}
}