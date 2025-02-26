using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using TaxiDC2.Interfaces;

namespace TaxiDC2.ViewModels;

public partial class TripListViewModel : BaseViewModel, IDisposable
{
	private IBussinessState _bs;
	private readonly IPlaySoundService _soundService;

	public ObservableCollection<TripListItemViewModel> Items { get; } =
		new ObservableCollection<TripListItemViewModel>();

	private Timer timer = null;

	public TripListViewModel(IBussinessState bs, IPlaySoundService soundService, IDataService dataService) : base(dataService)
	{
		_bs = bs;
		_soundService = soundService;

		timer = new Timer(OnTimer, null, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(15));

		//New trip
		MessagingCenter.Subscribe<INotificationMessage>(this, "MES", (sender) => { Task.Run(async () => await OnMessage(sender)).Wait(); });
	}

	private async Task OnMessage(INotificationMessage sender)
	{
		await RefreshData();
		NotifyMessageData md = JsonConvert.DeserializeObject<NotifyMessageData>(sender.MessageData);
		System.Diagnostics.Debug.WriteLine($"List page : message ; {md.title} - {md.body}");
		if (md.msg == MessageType.FWD.ToString())
		{
			// Predani jizdy na ridice
			IDictionary<string, Guid> dd = JsonConvert.DeserializeObject<IDictionary<string, Guid>>(md.data);
			if (dd != null && dd["driver"] == _bs.DriverId)
			{
				string t = dd["trip"].ToString();
				// jsem prijemce 
				MainThread.BeginInvokeOnMainThread(async () =>
				{
					await Shell.Current.GoToAsync($"{nameof(TripAlert)}?IdTrip={t}");
					_soundService.PlaySystemSound("");
				});
			}
		}
	}

	public void Dispose()
	{
		//unsubscribe Messages
		MessagingCenter.Unsubscribe<INotificationMessage>(this, "MES");
		//clear resources
		timer?.Dispose();
	}

	/// <summary>
	/// Nastavuje list na vsechno 0 nebo privatni { jen moje} 1
	/// </summary>
	[ObservableProperty]
	private int _listMode = 0;

	public string DriverName => $"{_bs?.Driver?.Inicials ?? "XX"}";

	public void OnAppearing()
	{
		IsBusy = true;
	}

	internal async Task RefreshData()
	{
		try
		{
			Items.Clear();
			IEnumerable<TripListItemViewModel> l = (await DataService.GetTripAsync(true)).Select(TripListItemViewModel.FromTrip);
			if (ListMode != 0) // jen moje
				l = l.Where(w =>
					w.Data.TripState is (TripState.NewOrder or TripState.RejectedByDiver) ||
					w.Data.Driver?.IdDriver == _bs.DriverId);

			foreach (var item in l
						 .OrderBy(o => o.Data.TripState != TripState.NewWWW)
						 .ThenBy(o => (int)o.Data.TripState > 99)
						 .ThenBy(o => o.MinToDeadLine))
				Items.Add(item);
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			IsBusy = false;
		}
		finally
		{
			IsBusy = false;
		}
	}

	[RelayCommand]
	private async Task LoadItems()
	{
		await RefreshData();
	}
	
	private void OnTimer(object state)
	{
		foreach (TripListItemViewModel item in Items)
		{
			item.RefreshTime();
		}
	}

	[RelayCommand]
	public async Task TripAdd()
	{
		await Shell.Current.GoToAsync(nameof(NovaJizda));

	}

	[RelayCommand]
	public async Task ProfileOpen()
	{
		if (_bs?.DriverId != null)
			await Shell.Current.GoToAsync($"{nameof(DriverName)}?Id={_bs.DriverId}");
	}

	[RelayCommand]
	private async Task Storno(Guid idTrip)
	{
		if (!await Shell.Current.DisplayAlert("STORNO", "Opravdu zrušit jízdu ?", "ANO","NE"))
		{
			return;
		};

		var ret = await DataService.ChangeTripStateAsync(idTrip, TripState.Canceled);
		if (ret)
		{
			await RefreshData();
		}
		else
		{
			await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla stornována", "OK");
		}
	}

	[RelayCommand]
	private async Task Acc(Guid idTrip)
	{
		if (_bs.DriverId == null)
		{
			await Shell.Current.DisplayAlert("ERROR", "No active driver", "OK");
			return;
		}

		var ret = await DataService.AcceptTripByDriverAsync(idTrip, _bs.DriverId.Value);
		if (ret)
		{
			await RefreshData();
		}
		else
		{
			await Shell.Current.DisplayAlert("POZOR", "Jízda nebyla akceptována", "OK");
		}
	}
}
