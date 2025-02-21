using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace TaxiDC2.ViewModels
{
	public partial class DriverListViewModel : BaseViewModel
	{
		private readonly IBussinessState _bs;
		private Driver _selectedItem;

		public ObservableCollection<Driver> Items { get; }
		
		public bool IsAdmin => _bs.IsAdmin;

		public DriverListViewModel(IBussinessState bs, IDataService dataService) : base(dataService)
		{
			_bs = bs;
			Items = new ObservableCollection<Driver>();
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		[RelayCommand]
		public async Task SaveToggleData(Driver driver)
		{
			IsBusy = true;
			try
			{
				var ret = await DataService.UpdateDriverSettingsAsync(driver);
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
		async Task LoadData()
		{
			IsBusy = true;
			try
			{
				Items.Clear();
				var result = await DataService.GetDriversAsync(true);
				foreach (Driver item in result.OrderBy(o => o.FullName))
				{
					Items.Add(item);
					item.IsDirty = false;
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

		[RelayCommand]
		async void ItemSelected(Guid? id)
		{
			if (id == null) return;

			await Shell.Current.GoToAsync($"{nameof(DetailRidic)}?id={id}");
		}

		[RelayCommand]
		private async Task ActiveToggled(Guid id)
		{
			if (id == null) return;
			var driver = Items.FirstOrDefault(f => f.IdDriver == id);
			if (driver != null)
			{
				//driver.Active = !driver.Active;
				driver.IsDirty = true;
				await SaveToggleData(driver);
			}
		}

		[RelayCommand]
		private async Task NotifiToggled(Guid id)
		{
			if (id == null) return;
			var driver = Items.FirstOrDefault(f => f.IdDriver == id);
			if (driver != null)
			{
				//driver.NotificationEnable = !driver.NotificationEnable;
				driver.IsDirty = true;
				await SaveToggleData(driver);
			}
		}

		[RelayCommand]
		private async void AddItem(object obj)
		{
			await Shell.Current.GoToAsync(nameof(DetailRidic));
		}
	}
}