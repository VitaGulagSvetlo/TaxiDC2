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
        
        // Handle the swipe action to navigate to another page
        private async void OnSwipeRightItemInvoked(object sender, EventArgs e)
        {
	        var swipeItem = (SwipeItem)sender;
	        var id = swipeItem.CommandParameter;
	        Shell.Current.GoToAsync($"{nameof(DetailJizda)}?id={id.ToString()}");
        }

        private async void OnSwipeLeftItemInvoked(object sender, EventArgs e)
        {
	        var swipeItem = (SwipeItem)sender;
	        var id = swipeItem.CommandParameter;
	        Shell.Current.GoToAsync($"{nameof(DetailJizda)}?id={id.ToString()}");
        }
    }
}
