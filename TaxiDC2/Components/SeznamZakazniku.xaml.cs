using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamZakazniku : ContentPage
    {
        readonly CustomerListViewModel _viewModel;

        public SeznamZakazniku(CustomerListViewModel model)
        {
            InitializeComponent();
            BindingContext = _viewModel = model;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
        
        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
        
    }
}
