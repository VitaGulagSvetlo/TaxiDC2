using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TaxiDC2.ViewModels
{
    public class DriverListViewModel : BaseViewModel
    {
        private readonly IApiProxy _proxy = DependencyService.Get<IApiProxy>();
        private readonly IBussinessState _bs = DependencyService.Get<IBussinessState>();

        private Driver _selectedItem;

        public ObservableCollection<Driver> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Driver> ItemTapped { get; }
        public DriverListViewModel()
        {
            Title = "Řidiči";
            Items = new ObservableCollection<Driver>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Driver>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

        }
        
        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            IsAdmin = _bs.IsAdmin;
            try
            {
                Items.Clear();
                var result = await _proxy.GetDriversAsync(true);
                if (result.State == ResultCode.OK)
                {
                    foreach (Driver item in result.Data.OrderBy(o=>o.FullName))
                    {
                        Items.Add(item);
                        item.IsDirty = false;
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

        public Driver SelectedItem
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
            //await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Driver item)
        {
            if (item == null)
                return;

             //await Shell.Current.GoToAsync($"{nameof(DriverReg)}?Id={item.IdDriver}");
        }
       
        public async Task SaveToggleData()
        {
            try
            {
                foreach (Driver driver in Items.Where(w=>w.IsDirty))
                {
                    ServiceResult ret = await _proxy.UpdateDriverSetings(driver);
                    if (ret.State==ResultCode.OK)
                        driver.IsDirty = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}