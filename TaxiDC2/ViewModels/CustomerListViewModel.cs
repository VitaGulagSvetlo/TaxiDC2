using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace TaxiDC2.ViewModels
{
    public partial class CustomerListViewModel : BaseViewModel
    {
        public ObservableCollection<Customer> Items { get; }= new ObservableCollection<Customer>();

        public CustomerListViewModel(IDataService dataService) : base(dataService)
		{
            Title = "Zákazníci";
        }

        private async Task RefreshData()
        {
	        IsBusy = true;
	        try
	        {
		        Items.Clear();
		        var result = await DataService.FindCustomersAsync(SearchText);
		        foreach (var item in result.OrderBy(o => o.Name))
		        {
			        Items.Add(item);
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
        }

        public string SearchText { get; set; } = " ";

        [RelayCommand]
        private async void AddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(DetailZakaznik));
        }

		[RelayCommand]
		async void ItemSelected(Guid? id)
        {
            if (id == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(DetailZakaznik)}?id={id}");
        }

        [RelayCommand]
        public async Task Search()
        {
            await RefreshData();
        }

        [RelayCommand]
        async Task LoadData()
        {
	        IsBusy = true;

	        try
	        {
		        await RefreshData();
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

        [RelayCommand]
        public async Task SaveToggleData(Customer customer)
        {
	        try
	        {
		        var ret = await DataService.UpdateCustomerSettingsAsync(customer);
	        }
	        catch (Exception ex)
	        {
		        Debug.WriteLine(ex);
	        }
        }


	}
}