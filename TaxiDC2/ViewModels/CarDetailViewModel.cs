using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using TaxiDC2.Interfaces;

namespace TaxiDC2.ViewModels
{
	
	public partial class CarDetailViewModel : BaseViewModel
    {
        
        private readonly IDataService _dataService;

        public CarDetailViewModel(IDataService dataService) : base(dataService)
		{
	        _dataService = dataService;
        }

        [ObservableProperty]
        private Car _car  = new Car();

		[RelayCommand]
		private async Task SaveData()
        {
            var ret = await _dataService.SaveCarAsync(_car);

			if (ret)
            {
	            await Shell.Current.DisplayAlert("Cars", "Auto uloženo", "OK");
				await Shell.Current.GoToAsync($"..");
			}
			else
            {
	            //Message = "Chyba ukládání auta";
			}
		}

		[RelayCommand]
		public async Task LoadData(Guid id)
        {
	        _car = await _dataService.GetCarByIdAsync(id);
		}
    }
}