using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TaxiDC2.ViewModels
{
    public class CarListViewModel : BaseViewModel
    {
        private readonly IApiProxy _proxy = DependencyService.Get<IApiProxy>();

        private Car _selectedItem;

        public ObservableCollection<Car> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Car> ItemTapped { get; }

        public CarListViewModel()
        {
            Title = "Vozy";
            Items = new ObservableCollection<Car>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Car>(OnItemSelected);
            AddItemCommand = new Command(OnAddItem);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var result = await _proxy.GetCarsAsync(true);
                if (result.State == ResultCode.OK)
                {
                    foreach (Car item in result.Data)
                    {
                        Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Car SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(DetailAuto));
        }

        private async void OnItemSelected(Car item)
        {
            if (item == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(DetailAuto)}?IdCar={item.IdCar}");

        }
    }
}