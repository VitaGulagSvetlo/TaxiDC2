using Syncfusion.Maui.Buttons;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class SeznamJizd : ContentPage
    {
        readonly TripListViewModel _viewModel;

        public SeznamJizd(IBussinessState bs,TripListViewModel model)
        {
            InitializeComponent();
            BindingContext = _viewModel = model;
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
            if (Guid.TryParse(id.ToString(), out Guid g))
            {
	            _viewModel.StornoCommand.Execute(g);
            }
        }

		private async void OnSwipeLeftAccept(object sender, EventArgs e)
        {
            SwipeItem swipeItem = (SwipeItem)sender;
            object id = swipeItem.CommandParameter ?? throw new ArgumentNullException("swipeItem.CommandParameter");
            if(Guid.TryParse(id.ToString(),out Guid g))
			{
				_viewModel.AccCommand.Execute(g);
			}
		}

        private async void SfSwitch_OnStateChanged(object sender, SwitchStateChangedEventArgs e)
        {
	        _viewModel.ListMode = e.NewValue==true ? 1 : 0;
			await _viewModel.RefreshData();
		}
    }
}
