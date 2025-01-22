using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
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

	    protected override bool OnBackButtonPressed()
	    {
		    Shell.Current.GoToAsync($"///{nameof(SeznamJizd)}");
		    return true;
	    }

		private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            // Navigate to a new blank page
            await Navigation.PushAsync(new DetailAuto());
        }
    }
}
