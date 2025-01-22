using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamZakazniku : ContentPage
    {
        public ObservableCollection<CustomerDetailViewModel> Items { get; set; }

        public SeznamZakazniku()
        {
            InitializeComponent();
            Items = new ObservableCollection<CustomerDetailViewModel>
            {
                new CustomerDetailViewModel
                {
                    PhoneNumber = "1324568453",
                    Name = "Jakub",
                    VIP = false,
                    Rejected = false,

                },
                new CustomerDetailViewModel
                {
                   PhoneNumber = "1324568453",
                    Name = "Jakub",
                    VIP = false,
                    Rejected = false,
                },
                new CustomerDetailViewModel
                {
                    PhoneNumber = "1324568453",
                    Name = "Jakub",
                    VIP = false,
                    Rejected = true,

                },
                new CustomerDetailViewModel
                {
                    PhoneNumber = "1324568453",
                    Name = "Jakub",
                    VIP = true,
                    Rejected = false,

                },
                new CustomerDetailViewModel
                {
                    PhoneNumber = "1324568453",
                    Name = "Jakub",
                    VIP = false,
                    Rejected = false,

                }

            };

            listZakaznici.ItemsSource = Items;
        }
        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            // Navigate to a new blank page
            await Navigation.PushAsync(new DetailRidic());
        }

        // This method will be called when the cancel button is clicked

    }
}
