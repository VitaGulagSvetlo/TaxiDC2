using System.ComponentModel;

namespace TaxiDC2.ViewModels
{
    internal class SplashVm : INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;
    
        public bool ConfigVisible { get; set; }
        public bool NotActive { get; set; }
    }
}
