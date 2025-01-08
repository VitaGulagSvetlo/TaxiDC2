using Microsoft.Maui.Controls;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailZakaznik : ContentPage
    {
        public DetailZakaznik()
        {
            InitializeComponent();
            BindingContext = new CustomerDetailViewModel()
            {
                Name = "Peno",
				PhoneNumber = "123456789",
				LastAddressBoarding = "Praha",
				LastAddressExit = "Brno",
				Time = DateTime.Now,
				VIP = true,
				Rejected = false,
				Memo = "Poznamka",
				VIP2 = 2

			};
		}

        
    }
}
