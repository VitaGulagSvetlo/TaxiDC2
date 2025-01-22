using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamRidicu : ContentPage
    {
        readonly DriverListViewModel _viewModel;

        public SeznamRidicu(IApiProxy proxy, IBussinessState bs)
        {
            InitializeComponent();

            BindingContext = _viewModel = new DriverListViewModel(proxy, bs);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.GoToAsync($"///{nameof(SeznamJizd)}");
            return true;
        }
        
        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            // Navigate to a new blank page
            await Navigation.PushAsync(new DetailRidic());
        }
        // This method will be called when the cancel button is clicked
    }
}
