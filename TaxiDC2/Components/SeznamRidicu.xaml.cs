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
        
        private async void OnBackButtonPressed(object sender, EventArgs e)
        {

            Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");

        }

        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {

            Shell.Current.GoToAsync($"{nameof(DetailRidic)}");
        }


		private void Switch1_OnToggled(object sender, ToggledEventArgs e)
		{
			if (_viewModel != null)
				Task.Run(async () => { return _viewModel.SaveToggleData(); });
		}
	}
}
