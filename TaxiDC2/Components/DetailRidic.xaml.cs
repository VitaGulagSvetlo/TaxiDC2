using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using TaxiDC2.Models;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
	public partial class DetailRidic : ContentPage, IQueryAttributable
	{
		public DetailRidic(DriverNewViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;
			vm?.LoadData();
		}

		public void ApplyQueryAttributes(IDictionary<string, object> query)
		{
			if (query.ContainsKey("id"))
			{
				var idAsString = query["id"]?.ToString();
				if (Guid.TryParse(idAsString, out var parsedId))
				{
					
					var vm = BindingContext as DriverNewViewModel;
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