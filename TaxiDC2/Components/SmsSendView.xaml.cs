namespace TaxiDC2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(IdTrip), nameof(IdTrip))]
    [QueryProperty(nameof(Phone), nameof(Phone))]
    public partial class SmsSendView : ContentPage
    {
		public SmsSendView(ViewModels.SmsViewModel vm)
		{
			InitializeComponent();
			BindingContext = _viewModel = vm;
		}
        
		readonly ViewModels.SmsViewModel _viewModel;

        public string IdTrip
        {
            get => _viewModel.IdTrip;
            set
            {
                _viewModel.IdTrip = _viewModel.IdTrip;
                OnPropertyChanged();
            }
        }

        public string Phone
        {
            get => _viewModel.PhoneNumber;
            set
            {
                _viewModel.PhoneNumber = Uri.UnescapeDataString(value ?? string.Empty); 
                OnPropertyChanged();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void OnSwipe(object sender, SwipedEventArgs e)
        {
            await Shell.Current.GoToAsync($"///{nameof(DetailJizda)}?id={IdTrip}");
        }

        protected override bool OnBackButtonPressed()
        {
            Shell.Current.GoToAsync($"..");
            return true;
            //            return base.OnBackButtonPressed();
        }
    }
}