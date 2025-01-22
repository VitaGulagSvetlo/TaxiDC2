using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using TaxiDC2.Interfaces;

namespace TaxiDC2.ViewModels
{
    public class CarDetailViewModel : INotifyPropertyChanged
    {
        private IApiProxy _proxy;

        public CarDetailViewModel(IApiProxy proxy)
        {
            _proxy = proxy;
            Title = "Vozidlo";
            SaveDataCmd = new Command(async () => await SaveData());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get; set; }
        public string Message { get; set; } = "";
        public ICommand SaveDataCmd { get; }
        public string Title { get; set; } = string.Empty;

        public async Task SaveData()
        {
            Car cr = new()
            {
                IdCar = this.IdCar,
                CarType = this.CarType,
                Rz = this.Rz,
                Color = this.Color,
                Memo = this.Memo,
                IsDeleted = this.IsDeleted
            };

            ServiceResult ret = await _proxy.SaveCar(cr);
            Message = ret.Message;
            await Shell.Current.DisplayAlert("Ukládání", ret.Message, "OK");

			if (ret.State == ResultCode.OK)
            {
                // OK
            }
            else
            {
                //ERR
            }
        }

        public async Task LoadData(Guid id)
        {
	        var car = await _proxy.GetCarByIdAsync(id);
			if (car.State == ResultCode.OK && car.Data != null)
			{
				IdCar = car.Data.IdCar;
				CarType = car.Data.CarType;
				Rz = car.Data.Rz;
				Color = car.Data.Color;
				Memo = car.Data.Memo;
				IsDeleted = car.Data.IsDeleted;
			}
		}


		#region Data

		[StringLength(50)]
        [Required]
        public string CarType { get; set; }

        [StringLength(50)]
        [Required]
        public string Color { get; set; }

        public Guid IdCar { get; set; } = Guid.Empty;

        public bool IsDeleted { get; set; } = false;

        public bool IsDirty { get; set; } = false;

        public string Memo { get; set; }

        [StringLength(50)]
        [Required]
        public string Rz { get; set; }

        #endregion Data
    }
}