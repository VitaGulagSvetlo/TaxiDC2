using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamRidicu : ContentPage
    {
        readonly DriverListViewModel _viewModel;

        public SeznamRidicu(DriverListViewModel model)
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
			await Shell.Current.GoToAsync($"{nameof(MainPage)}");
		}
        
	}
}
