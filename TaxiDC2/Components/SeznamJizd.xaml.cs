using Microsoft.Maui.Controls;
using Syncfusion.Maui.ListView;
using System;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class SeznamJizd : ContentPage
    {
        readonly TripListViewModel _viewModel;

        public SeznamJizd(IApiProxy proxy, IBussinessState bs)
        {
            InitializeComponent();

            BindingContext = _viewModel = new TripListViewModel(proxy, bs);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

        }


        private async void OnBackButtonPressed(object sender, EventArgs e)
        {

            Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");

        }

        // Handle the swipe action to navigate to another page
        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            // Navigate to a new blank page
            Shell.Current.GoToAsync($"{nameof(DetailJizda)}");
        }
    }
}
