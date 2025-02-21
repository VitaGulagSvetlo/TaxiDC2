using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TaxiDC2.Interfaces;

namespace TaxiDC2.ViewModels
{
	public partial class TripNewViewModel : BaseViewModel
	{
		private IApiProxy _proxy;
		private readonly ICallLogService _callLogService;
		private IBussinessState _bs;

		private string _phone;
		private string _adresaStart;
		private string _adresaCil;
		private string _casNastupu;
		private int _deadLine = 0;
		private string _memo;
		private Customer _customer;
		private string custId;

		private ObservableCollection<CallListItem> _listCisel = new();
		private ObservableCollection<Lokace> _listLokaci = new();
		private bool _telPickerVisible = false;
		private bool _scPickerVisible = true;
		private TimeSpan? _casNastupuD;
		private bool _adrPickerVisible;
		private string adresaStartX;
		private string adresaStartY;
		private string adresaCilX;
		private string adresaCilY;
		private bool adresaStartValid;
		private bool adresacilValid;
		internal int _locSelector;
		private string kontakt;
		private string custMemo;

		public ObservableCollection<int> MinutesList => new ObservableCollection<int> { 5, 10, 15, 20, 25, 30 };

		public ICommand DeadlineClick { get; set; }
		public ICommand SaveCmd { get; }
		public ICommand SendSMS { get; }
		public ICommand NactiCislo { get; }
		public ICommand ValidujAdr1 { get; }
		public ICommand ValidujAdr2 { get; }


		public ObservableCollection<CallListItem> ListCisel => _listCisel;
		public ObservableCollection<Lokace> ListLokaci => _listLokaci;

		public TripNewViewModel(IDataService dataService, IBussinessState bs, IApiProxy proxy, ICallLogService callLogService) : base(dataService)
		{
			_bs = bs;
			_proxy = proxy;
			_callLogService = callLogService;
			SendSMS = new Command(async () => await PosliSms("test zprava", _phone));
			NactiCislo = new Command(async () => await NactiKontakty());
			ValidujAdr1 = new Command(async () => await ValidujAdresu(1));
			ValidujAdr2 = new Command(async () => await ValidujAdresu(2));
			DeadlineClick = new Command<string>(DeadlineSet);
			SaveCmd = new Command(async () => await SaveData());
		}

		private void DeadlineSet(string v)
		{
			if (int.TryParse(v, out int i))
				Deadline = i;
		}

		internal void NastavAdresu(Lokace sel)
		{
			if (_locSelector == 1)
			{
				AdresaStart = sel.Title;
				AdresaStartX = sel.X;
				AdresaStartY = sel.Y;
				AdresaStartValid = true;
			}
			else
			{
				AdresaCil = sel.Title;
				AdresaCilX = sel.X;
				AdresaCilY = sel.Y;
				AdresacilValid = true;
			}
		}

		public async Task SaveData()
		{
			if (_customer == null)
				SetContactFromPhone(_phone);

			_customer.LastAddressBoarding = _adresaStart;
			_customer.LastAddressExit = _adresaCil;

			Trip drv = new()
			{
				OrderTime = DateTime.Now,
				AddressBoarding = string.IsNullOrWhiteSpace(_adresaStart) ? "Nezadána" : _adresaStart,
				AddressExit = string.IsNullOrWhiteSpace(_adresaCil) ? "Nezadána" : _adresaCil,
				AddressBoardingIsValid = adresaStartValid,
				AddressBoardingLocX = adresaStartX,
				AddressBoardingLocY = adresaStartY,
				AddressExitIsValid = adresacilValid,
				AddressExitLocX = adresaCilX,
				AddressExitLocY = adresaCilY,

				BoardingTime = VypoctiCas(),
				Driver = null,
				DeadLine = TimeSpan.FromMinutes(_deadLine),
				TripState = TripState.NewOrder,
				Complete = false,
				Memo = _memo,
				Customer = _customer
			};

			var ret = await _proxy.SaveTrip(drv);
			Message = ret.Message;
			await Shell.Current.DisplayAlert("Ukládání", ret.Message, "OK");

			if (ret.State == ResultCode.OK)
			{
				// OK prechazim na detailni stranku
				await Shell.Current.GoToAsync($"{nameof(DetailJizda)}?id={ret.Data.IdTrip}");
			}
			else
			{
				//ERR
			}
		}

		private DateTime? VypoctiCas()
		{
			if (_casNastupuD == null)
				return DateTime.Now.AddMinutes(Deadline);


			DateTime den = DateTime.Now.Date;

			if (_casNastupuD < DateTime.Now.TimeOfDay)
				den = den.AddDays(1);

			return den + _casNastupuD;
		}

		/// <summary>
		/// Aktivuje picker pro posledni telefoni cisla
		/// </summary>
		/// <returns></returns>
		public async Task NactiKontakty()
		{
			try
			{
				var log = await _callLogService.GetCallLogEntriesAsync();

				_listCisel.Clear();

				foreach (var item in _listCisel)
					_listCisel.Add(new CallListItem() { Cislo = item.Cislo, Jmeno = item.Jmeno });

#if DEBUG
				_listCisel.Add(new CallListItem() { Cislo = "111 111 111", Jmeno = "Clovek 1" });
				_listCisel.Add(new CallListItem() { Cislo = "222 222 222", Jmeno = "Clovek 2" });
				_listCisel.Add(new CallListItem() { Cislo = "333 333 333", Jmeno = "Clovek 3" });
				_listCisel.Add(new CallListItem() { Cislo = "444 444 444", Jmeno = "Clovek 4" });
#endif

				OnPropertyChanged(nameof(ListCisel));
			}
			catch (Exception es)
			{
				Debug.WriteLine("Chyba nacteni kontaktu 2: " + es.Message);
			}
			finally
			{
				PhoneSelected = _listCisel.First().Cislo;
				TelPickerVisible = true;
			}
		}

		/// <summary>
		/// Aktivuje picker pro upresneni adresy
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		private async Task ValidujAdresu(int v)
		{
			_locSelector = v;
			try
			{
				{
					var lo = await _proxy.Geocode(v == 1 ? AdresaStart : AdresaCil);
					if (lo.State == ResultCode.ERROR)
						return;

					_listLokaci.Clear();
					lo.Data.ToList().ForEach(p => _listLokaci.Add(p));
					OnPropertyChanged(nameof(ListLokaci));

					if (lo.Data.Count() == 1) // pokud je jen jedna adresa tak hned nastav
					{
						NastavAdresu(lo.Data[0]);
					}
					else
						AdrPickerVisible = true;
				}
			}
			catch (Exception es)
			{
				Debug.WriteLine("Chyba nacteni lokaci : " + es.Message);
			}
		}

		public async Task PosliSms(string messageText, string recipient)
		{
			if (string.IsNullOrEmpty(_phone))
			{
				Debug.WriteLine("Není vyplneno telefoni cislo");
				return;
			}

			try
			{
				SmsMessage message = new(messageText, new[] { recipient });
				await Sms.ComposeAsync(message);
			}
			catch (FeatureNotSupportedException)
			{
				// Sms is not supported on this device.
				//await DisplayAlert("", $"Chyba \n{ex.Message}", "Aha");
			}
			catch (Exception)
			{
				// Other error has occurred.
			}
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		public string CustId
		{
			get => custId; set => SetProperty(ref custId, value);
		}

		public string TelCislo
		{
			get => _phone;
			set => SetProperty(ref _phone, value);
		}

		public Customer Customer
		{
			get => _customer;
			set => SetProperty(ref _customer, value);
		}

		public string AdresaStart
		{
			get => _adresaStart;
			set => SetProperty(ref _adresaStart, value);
		}

		public string AdresaCil
		{
			get => _adresaCil;
			set => SetProperty(ref _adresaCil, value);
		}

		public string CasNastupu
		{
			get => _casNastupu;
			set => SetProperty(ref _casNastupu, value);
		}

		public TimeSpan? CasNastupuD
		{
			get => _casNastupuD;
			set => SetProperty(ref _casNastupuD, value);
		}

		public int Deadline
		{
			get => _deadLine;
			set => SetProperty(ref _deadLine, value);
		}

		public string Memo
		{
			get => _memo;
			set => SetProperty(ref _memo, value);
		}

		public bool TelPickerVisible
		{
			get => _telPickerVisible;
			set
			{
				SetProperty(ref _telPickerVisible, value);
				ScPickerVisible = !value;
			}
		}

		public bool AdrPickerVisible
		{
			get => _adrPickerVisible;
			set
			{
				SetProperty(ref _adrPickerVisible, value);
				ScPickerVisible = !value;
			}
		}

		public bool ScPickerVisible
		{
			get => _scPickerVisible;
			set => SetProperty(ref _scPickerVisible, value);
		}

		public string AdresaStartX
		{
			get => adresaStartX; set => SetProperty(ref adresaStartX, value);
		}

		public string AdresaStartY
		{
			get => adresaStartY; set => SetProperty(ref adresaStartY, value);
		}

		public string AdresaCilX
		{
			get => adresaCilX; set => SetProperty(ref adresaCilX, value);
		}

		public string AdresaCilY
		{
			get => adresaCilY; set => SetProperty(ref adresaCilY, value);
		}

		public bool AdresaStartValid
		{
			get => adresaStartValid; set => SetProperty(ref adresaStartValid, value);
		}

		public bool AdresacilValid
		{
			get => adresacilValid; set => SetProperty(ref adresacilValid, value);
		}

		public short CustomerVIP => Customer?.VIP2 ?? 0;

		/// <summary>
		/// pomocna trida do ktere ukladam jmeno a cislo
		/// </summary>
		public class CallListItem : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;

			public string Cislo { get; set; }

			public string Jmeno { get; set; }
			public bool Missed { get; set; }
			public Color CallColor => Missed ? Color.Parse("DarkRed") : Color.FromHex("#ffbd00");
		}

		public async Task SetContactFromPhone(string cislo, string jmeno = "")
		{
			var res = await _proxy.GetCustomerByPhone(cislo);
			if (res.State == ResultCode.OK && res.Data != null)
			{
				Customer = res.Data;
				TelCislo = res.Data.PhoneNumber;
			}
			else
			{
				Customer = new Customer()
				{
					IdCustomer = Guid.Empty,
					Name = jmeno ?? "",
					PhoneNumber = cislo
				};
				TelCislo = cislo;
			}
		}

		public string CasTxt => VypoctiCas()?.ToShortTimeString();

		public bool CasVisible => _casNastupuD != null || Deadline > 0;
		public string[] ListCisel2 => ListCisel.Select(s => s.Cislo).ToArray();

		public string PhoneSelected { get; set; }

		[RelayCommand]
		public async Task PhoneAccept()
		{
			await SetContactFromPhone(PhoneSelected);
			TelPickerVisible = false;
		}

		[RelayCommand]
		public async Task PhoneCancel()
		{
			TelPickerVisible = false;
		}
	}

}