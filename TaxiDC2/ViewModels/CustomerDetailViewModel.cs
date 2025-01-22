using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using TaxiDC2.Interfaces;

namespace TaxiDC2.ViewModels
{
    public class CustomerDetailViewModel : INotifyPropertyChanged
    {
        private readonly IApiProxy _proxy = DependencyService.Get<IApiProxy>();

        public CustomerDetailViewModel()
        {
            Title = "Zákazník";
            SaveDataCmd = new Command(async () => await SaveData());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get; set; }
        public string Message { get; set; } = "";
        public ICommand SaveDataCmd { get; }
        public string Title { get; set; } = string.Empty;

        public async Task SaveData()
        {
            Customer cu = new()
            {
                IdCustomer = this.IdCustomer,
                PhoneNumber = this.PhoneNumber,
                LastAddressBoarding = this.LastAddressBoarding,
                LastAddressExit =  this.LastAddressExit,
                Time = this.Time,   
                VIP = this.VIP,
                Rejected = this.Rejected,
                Memo = this.Memo,
                VIP2 = this.VIP2,
                Name = this.Name
            };

            ServiceResult ret = await _proxy.SaveCustomer(cu);
            Message = ret.Message;
            await Shell.Current.DisplayAlert("Ukládání", ret.Message, "Cancel");
            if (ret.State == ResultCode.OK)
            {
                // OK
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                //ERR
            }
        }

        #region Data

        public Guid IdCustomer { get; set; } = Guid.Empty;
        [StringLength(500)]
        public string? LastAddressBoarding { get; set; }

        [StringLength(500)]
        public string? LastAddressExit { get; set; }

        public string Memo { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }
        public bool Rejected { get; set; }
        public DateTime Time { get; set; }
        public bool VIP { get; set; }
        public short VIP2 { get; set; } = 0;
        
        [DependsOn("VIP2")]
        public bool St1Visible => VIP2 > 0;
        [DependsOn("VIP2")]
        public bool St2Visible => VIP2 > 1;
        [DependsOn("VIP2")]
        public bool St3Visible => VIP2 > 2;
        [DependsOn("VIP2")]
        public bool St4Visible => VIP2 > 3;
        [DependsOn("VIP2")]
        public bool St5Visible => VIP2 > 4;

        #endregion Data
    }
}