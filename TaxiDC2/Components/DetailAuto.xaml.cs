using Microsoft.Maui.Controls;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailAuto : ContentPage, IQueryAttributable
	{
        public DetailAuto(CarDetailViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
	        if (query.ContainsKey("id"))
	        {
		        var idAsString = query["id"]?.ToString();
		        if (Guid.TryParse(idAsString, out var parsedId))
		        {
			        
			        var vm = BindingContext as CarDetailViewModel;
			        vm?.LoadData(parsedId);
		        }
	        }
        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        Shell.Current.GoToAsync($"///{nameof(SeznamAut)}");
        }



	}
}
