using Microsoft.Maui.Controls;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailJizda : ContentPage, IQueryAttributable
	{
		private readonly IApiProxy _proxy;

		public DetailJizda(TripDetailViewModel vm, IApiProxy proxy)
        {
	        _proxy = proxy;
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
			        
			        var vm = BindingContext as TripDetailViewModel;
			        vm?.LoadData(parsedId,_proxy);
		        }
	        }
        }

        private async void OnBackButtonPressed(object sender, EventArgs e)
        {
	        Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");
        }


	}
}
