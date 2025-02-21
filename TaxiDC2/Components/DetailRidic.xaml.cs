using TaxiDC2.ViewModels;

namespace TaxiDC2
{
	public partial class DetailRidic : ContentPage, IQueryAttributable
	{
		public DetailRidic(DriverDetailViewModel vm)
		{
			InitializeComponent();
			Task.Run(async()=>await vm?.LoadData()!).Wait();
			BindingContext = vm;
		}

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.ContainsKey("id"))
			{
				var idAsString = query["id"]?.ToString();
				if (Guid.TryParse(idAsString, out var parsedId))
				{
					var vm = BindingContext as DriverDetailViewModel;
					vm?.LoadDataById(parsedId);
				}
			}
		}
		
		private async void OnBackButtonPressed(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(SeznamRidicu)}");
		}
	}
}