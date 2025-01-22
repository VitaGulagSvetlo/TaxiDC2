using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using TaxiDC2.Models;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailRidic : ContentPage
    {
        public ObservableCollection<Car> Cars { get; set; }
        

        public DetailRidic()
        {
            InitializeComponent();

            BindingContext = new DriverViewModel()
            {
                FirstName = "Vita",
                LastName = "svetelak",
                PhoneNumber = "732485969",
                Active = true,
                IsAdmin = true,
                NotificationEnable = false,
                


            };

        }
    }
}