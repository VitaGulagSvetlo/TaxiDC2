using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaxiDC2.ViewModels
{
	public partial class DriverDetailViewModel : BaseViewModel
	{

		public DriverDetailViewModel(IDataService dataService) : base(dataService)
		{
			Task.Run(async () => await LoadData()).Wait(); // naceni dat do listu aut
		}

		[ObservableProperty]
		private Driver _driver = new Driver();

		public ObservableCollection<Car> CarsList { get; set; } = new();

		public Car SelectedCar { get; set; }

		public async Task LoadData()
		{
			Car[] cars = await DataService.GetCarsAsync();
			CarsList.Clear();
			foreach (Car car in cars)
			{
				CarsList.Add(car);
				if (car.IdCar == Driver.AssignedCar)
					SelectedCar = car;
			}
		}

		[RelayCommand]
		public async Task SaveData()
		{
			
			bool ret;
			if (Driver.IdDriver == Guid.Empty)
				ret = await DataService.RegisterDriverAsync(Driver);
			else
				ret = await DataService.SaveDriverAsync(Driver);

			if (ret)
			{
				await Shell.Current.DisplayAlert("Řidiči", "Řidič uložen", "OK");
				await Shell.Current.GoToAsync($"//{nameof(SeznamRidicu)}");
			}
			else
			{
				//ERR
			}
		}
		
		public void OnAppearing()
		{
			IsBusy = true;
		}

		public async Task LoadDataById(Guid parsedId)
		{
			await LoadData();
			Driver driver = await DataService.GetDriverByIdAsync(parsedId);
			if ( driver != null)
			{
				Driver = driver??new Driver();
				SelectedCar = driver.Car;
			}
		}
	}
}