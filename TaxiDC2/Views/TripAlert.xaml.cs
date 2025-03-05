using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class TripAlert : ContentPage
    {
	    private readonly TripDetailViewModel _model;

	    public TripAlert( TripDetailViewModel model)
	    {
		    InitializeComponent();
		    BindingContext = _model = model;
	    }

	    public void ApplyQueryAttributes(IDictionary<string, object> query)
	    {
		    if (query.ContainsKey("id"))
		    {
			    var idAsString = query["id"]?.ToString();
			    if (Guid.TryParse(idAsString, out var parsedId))
			    {
				    var vm = BindingContext as TripDetailViewModel;
				    vm?.LoadData(parsedId);
			    }
		    }
	    }

		// This method will be called when the cancel button is clicked
		private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (SeznamJizd)
            await Navigation.PopAsync();
        }
    }
}
