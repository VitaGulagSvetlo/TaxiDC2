using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamZakazniku : ContentPage
    {
        readonly CustomerListViewModel _viewModel;

        public SeznamZakazniku(IApiProxy proxy)
        {
            InitializeComponent();
            
            BindingContext = _viewModel = new CustomerListViewModel(proxy);
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
        
        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
	        var swipeItem = (SwipeItem)sender;
	        var id = swipeItem.CommandParameter;
	        Shell.Current.GoToAsync($"{nameof(DetailZakaznik)}?id={id.ToString()}");
        }

        // This method will be called when the cancel button is clicked

    }
}
