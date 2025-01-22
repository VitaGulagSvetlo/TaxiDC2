using Microsoft.Maui.Controls;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailZakaznik : ContentPage
    {
        readonly CustomerDetailViewModel _viewModel;

        public DetailZakaznik(IApiProxy proxy)
        {
            InitializeComponent();

            BindingContext = _viewModel = new CustomerDetailViewModel(proxy);
        }

        


        private async void OnBackButtonPressed(object sender, EventArgs e)
        {

            Shell.Current.GoToAsync($"///{nameof(SeznamZakazniku)}");

        }

        
        


    }
}
