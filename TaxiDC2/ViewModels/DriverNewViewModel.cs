using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace TaxiDC2.ViewModels
{
    public class DriverNewViewModel : INotifyPropertyChanged
    {
        private readonly IApiProxy _proxy = DependencyService.Get<IApiProxy>();
        private readonly IBussinessState _bs = DependencyService.Get<IBussinessState>();

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get; set; }

        public string Title { get; private set; } = string.Empty;

        public DriverNewViewModel()
        {
            Title = "Řidič";
            SaveDataCmd = new Command(async () => await SaveData());
            LoadDataCmd = new Command(async () => await LoadData());
        }

        public async Task LoadData()
        {
            if (string.IsNullOrEmpty(_bs.DeviceKey)) return;
            ServiceResult<Driver> res = await _proxy.GetDriverByDeviceKeyAsync(_bs.DeviceKey, _bs.DeviceHash);
            if (res.State == ResultCode.OK && res.Data != null)
            {
                FirstName = res.Data.FirstName;
                LastName = res.Data.LastName;
                PhoneNumber = res.Data.PhoneNumber;
                MobileDeviceKey = res.Data.MobileDeviceKey;
                Active = res.Data.Active;
                NotificationEnable = res.Data.NotificationEnable;
                IdDriver = res.Data.IdDriver;
                AssignedCar = res.Data.AssignedCar;
                Car = res.Data.Car;
                MobileDeviceHash = res.Data.MobileDeviceHash;
                IsAdmin = res.Data.IsAdmin;
            }

            var cars = await _proxy.GetCarsAsync();
            if (cars.State == ResultCode.OK)
                foreach (Car car in cars.Data)
                {
                    CarsList.Add(car);
                    if (car.IdCar == AssignedCar)
                        SelectedCar = car;
                }

        }

        public string? MobileDeviceHash { get; set; }

        public ObservableCollection<Car> CarsList { get; set; } = new();

        public Car SelectedCar { get; set; }

        public async Task SaveData()
        {
            Driver drv = new()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                MobileDeviceKey = _bs.DeviceKey,
                PhoneNumber = string.IsNullOrWhiteSpace(this.PhoneNumber) ? "000 000 000" : this.PhoneNumber,
                NotificationEnable = this.NotificationEnable,
                Active = this.Active,
                IdDriver = this.IdDriver,
                AssignedCar = SelectedCar?.IdCar,
                MobileDeviceHash = _bs.DeviceHash,
                IsAdmin = this.IsAdmin
            };

            ServiceResult ret;
            if (IdDriver==Guid.Empty)
                ret = await _proxy.RegisterDriver(drv);
            else
                ret = await _proxy.SaveDriver(drv);
            _bs.ReloadDriver();
            Message = ret.Message;
            DependencyService.Get<IAlertMessage>().ShortAlert(ret.Message);
            if (ret.State == ResultCode.OK)
            {
                await LoadData();
                App.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync($"///TripListPage");
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

        public ICommand SaveDataCmd { get; }
        public ICommand LoadDataCmd { get; }

        #region Data

        public Guid IdDriver { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileDeviceKey { get; set; } = "";
        public bool Active { get; set; }
        public bool NotificationEnable { get; set; }
        public Car Car { get; set; }
        public Guid? AssignedCar { get; set; } = null;

        #endregion

        public string Message { get; set; } = "";
        
        [DependsOn("MobileDeviceKey")]
        public bool NoKey => string.IsNullOrWhiteSpace(_bs.DeviceKey);

        public bool IsAdmin { get; set; } = false;
    }
}