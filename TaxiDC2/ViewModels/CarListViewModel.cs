using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace TaxiDC2.ViewModels
{
	public partial class CarListViewModel : BaseViewModel
	{

		public ObservableCollection<Car> Items { get; }

		public CarListViewModel(IDataService dataService) : base(dataService)
		{
			Items = new ObservableCollection<Car>();
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		[RelayCommand]
		private async Task LoadData()
		{
			IsBusy = true;

			try
			{
				Items.Clear();
				Car[] result = await DataService.GetCarsAsync(true);
				foreach (Car item in result.OrderByDescending(o=>o.DateCreated))
					Items.Add(item);
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
		private async void AddItem(object obj)
		{
			await Shell.Current.GoToAsync(nameof(DetailAuto));
		}

		[RelayCommand]
		private async void ItemSelected(Guid id)
		{
			if (id == null)
				return;

			await Shell.Current.GoToAsync($"{nameof(DetailAuto)}?id={id}");

		}
	}
}