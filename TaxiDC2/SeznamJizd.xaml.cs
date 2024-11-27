using Microsoft.Maui.Controls;
using Syncfusion.Maui.ListView;
using System;
using System.Collections.ObjectModel;
using TaxiDC2.Models;

namespace TaxiDC2
{
    public partial class SeznamJizd : ContentPage
    {
        public ObservableCollection<Trip> Items { get; set; }

        public SeznamJizd()
        {
            InitializeComponent();
            Items = new ObservableCollection<Trip>
            {
                new Trip
                {
                    AddressBoarding = "ff",
                    AddressExit = "dwad",
                    OrderTime = DateTime.Now,
                },
                new Trip
                {
                    AddressBoarding = "ff",
                    AddressExit = "dwad",
                    OrderTime = DateTime.Now,
                },
                new Trip
                {
                    AddressBoarding = "ff",
                    AddressExit = "dwad",
                    OrderTime = DateTime.Now,
                }
            };

            listView.ItemsSource = Items;
        }

        // Handle the swipe action to navigate to another page
        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            // Navigate to a new blank page
            await Navigation.PushAsync(new DetailJizda());
        }
    }
}
