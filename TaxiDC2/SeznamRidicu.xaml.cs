using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamRidicu : ContentPage
    {
        public ObservableCollection<DriverViewModel> Items { get; set; }
        public SeznamRidicu()
        {
            InitializeComponent();
            Items = new ObservableCollection<DriverViewModel>
            {
                new DriverViewModel
                {
                    FirstName = "Vita",
                    LastName = "Svetlo",
                    PhoneNumber = "745845962",
                    Active = true,

                    
                }
               

            };

            listRidici.ItemsSource = Items;
        }
        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            // Navigate to a new blank page
            await Navigation.PushAsync(new DetailRidic());
        }
        // This method will be called when the cancel button is clicked
    }
}
