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
	        SwipeItem swipeItem = (SwipeItem)sender;
	        object id = swipeItem.CommandParameter;
	        await Shell.Current.GoToAsync($"{nameof(DetailJizda)}?id={id.ToString()}");
        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }

		private async void OnSwipeLeftCancel(object sender, EventArgs e)
        {
	        SwipeItem swipeItem = (SwipeItem)sender;
	        object id = swipeItem.CommandParameter ?? throw new ArgumentNullException("swipeItem.CommandParameter");
	        TripDetailViewModel m = _viewModel.Items.FirstOrDefault(f => f.IdTrip.ToString() == id.ToString());
	        if (m != null)
	        {
		        _viewModel.Items.Remove(m);

				//await m.CancelTrip();
			}
		}

		private async void OnSwipeLeftAccept(object sender, EventArgs e)
        {
	        SwipeItem swipeItem = (SwipeItem)sender;
	        object id = swipeItem.CommandParameter ?? throw new ArgumentNullException("swipeItem.CommandParameter");
	        TripDetailViewModel m = _viewModel.Items.FirstOrDefault(f => f.IdTrip.ToString() == id.ToString());
	        if (m != null)
	        {
		        await m.AcceptTrip();
	        }
        }
	}
}
