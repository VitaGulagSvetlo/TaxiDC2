using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
using ItemTappedEventArgs = Syncfusion.Maui.ListView.ItemTappedEventArgs;

namespace TaxiDC2
{
    public partial class SeznamAut : ContentPage
    {
	    readonly CarListViewModel _viewModel;

	    public SeznamAut(CarListViewModel model)
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
            await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
        }
    }
}
