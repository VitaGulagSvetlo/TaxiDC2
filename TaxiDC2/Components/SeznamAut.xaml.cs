using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
using ItemTappedEventArgs = Syncfusion.Maui.ListView.ItemTappedEventArgs;

namespace TaxiDC2
{
    public partial class SeznamAut : ContentPage
    {
	    private readonly IApiProxy _proxy;
	    readonly CarListViewModel _viewModel;

	    public SeznamAut(IApiProxy proxy)
	    {
		    _proxy = proxy;
		    InitializeComponent();
		    BindingContext = _viewModel = new CarListViewModel(proxy);
	    }

	    protected override void OnAppearing()
	    {
		    base.OnAppearing();
		    _viewModel.OnAppearing();

		}

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"///{nameof(SeznamJizd)}");
        }

        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"///{nameof(DetailAuto)}");
        }

        private void ListAut_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
			Shell.Current.GoToAsync($"{nameof(DetailAuto)}?id={((Car)e.DataItem).IdCar}");
		}
    }
}
