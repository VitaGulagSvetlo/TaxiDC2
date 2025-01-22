using System.Collections.ObjectModel;
using System.Diagnostics;
using Newtonsoft.Json;
using TaxiDC2.Interfaces;

namespace TaxiDC2.ViewModels
{
    public class TripListViewModel : BaseViewModel, IDisposable
    {
        private IBussinessState _bs;
        private IApiProxy _proxy;
        private TripDetailViewModel _selectedItem;

        private Timer timer = null;

        public TripListViewModel(IApiProxy proxy, IBussinessState bs)
        {
            _proxy = proxy;
            _bs = bs;
            Title = "Jízdy";
            Items = new ObservableCollection<TripDetailViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<TripDetailViewModel>(OnItemSelected);
            ItemSwipedR = new Command<TripDetailViewModel>(OnItemSwipedRight);
            ItemSwipedL = new Command<TripDetailViewModel>(OnItemSwipedLeft);
            ResetTime = new Command<TripDetailViewModel>(OnReset); // jen p[ro ladeni
            AddItemCommand = new Command(OnAddItem);
            timer = new Timer(OnTimer, null, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(15));

            //New trip
            MessagingCenter.Subscribe<INotificationMessage>(this, "MES",
                (sender) => { Task.Run(async () => await OnMessage(sender)).Wait(); });

        }

        private async Task OnMessage(INotificationMessage sender)
        {
            await RefreshData();
            NotifyMessageData md = JsonConvert.DeserializeObject<NotifyMessageData>(sender.MessageData);
            System.Diagnostics.Debug.WriteLine($"List page : message ; {md.title} - {md.body}");
            if (md.msg == MessageType.FWD.ToString())
            {
                // Predani jizdy na ridice
                var dd = JsonConvert.DeserializeObject<IDictionary<string, Guid>>(md.data);
                if (dd != null && dd["driver"] == _bs.DriverId)
                {
                    string t = dd["trip"].ToString();
                    // jsem prijemce 
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.GoToAsync($"{nameof(TripAlert)}?IdTrip={t}");
                        DependencyService.Get<IPlaySoundService>().PlaySystemSound();
                    });
                }
            }
        }

        public Command AddItemCommand { get; }
        public string DriverName => $"{_bs?.Driver?.Inicials}";
        public ObservableCollection<TripDetailViewModel> Items { get; }
        public Command<TripDetailViewModel> ItemSwipedL { get; }
        public Command<TripDetailViewModel> ItemSwipedR { get; }
        public Command<TripDetailViewModel> ItemTapped { get; }
        /// <summary>
        /// Nastavuje list na vsechno 0 nebo privatni { jen moje} 1
        /// </summary>
        public int ListMode { get; set; } = 0;

        public Command LoadItemsCommand { get; }
        public Command ModeSwitch { get; }
        public Command ResetTime { get; }
        public TripDetailViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public void Dispose()
        {
            //unsubscribe Messages
            MessagingCenter.Unsubscribe<INotificationMessage>(this, "MES");
            //clear resources
            timer?.Dispose();
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        internal async Task RefreshData()
        {

            try
            {
                Items.Clear();
                ServiceResult<Trip[]> result = await _proxy.GetTripsAsync(true);
                if (result.State == ResultCode.OK)
                {
                    var l = result.Data.Select(TripDetailViewModel.FromTrip);

                    if (ListMode != 0) // jen moje
                        l = l.Where(w => w.TripState is  (TripState.NewOrder or TripState.RejectedByDiver) || w.Driver?.IdDriver == _bs.DriverId);

                    foreach (TripDetailViewModel item in l
                                 .OrderBy(o => o.TripState != TripState.NewWWW)
                                 .ThenBy(o => (int)o.TripState > 99)
                                 .ThenBy(o => o.MinToDeadLine))
                        Items.Add(item);
                }
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

        async Task ExecuteLoadItemsCommand()
        {
            await RefreshData();
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NovaJizda));
        }

        async void OnItemSelected(TripDetailViewModel item)
        {
            if (item == null)
                return;
            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(DetailJizda)}?id={item.IdTrip}");
        }

        private async void OnItemSwipedLeft(TripDetailViewModel item)
        {
            if (item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(DetailJizda)}?id={item.IdTrip}");
        }

        private async void OnItemSwipedRight(TripDetailViewModel item)
        {
            if (item == null)
                return;
            TripDetailViewModel i = Items.First(f => f.IdTrip == item.IdTrip);
            if (i != null)
                Items.Remove(i);
        }

        private void OnReset(TripDetailViewModel obj)
        {
            foreach (TripDetailViewModel it in Items)
            {
                it.OrderTime = DateTime.Now.AddMinutes(-1);
            }
        }
        private void OnTimer(object state)
        {
            foreach (TripDetailViewModel item in Items)
            {
                item.RefreshTime();
            }
            //OnPropertyChanged(nameof(Items));
        }
    }
}