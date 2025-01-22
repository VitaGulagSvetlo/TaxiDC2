using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.ViewModels;
namespace TaxiDC2
{
    public partial class SeznamAut : ContentPage
    {
        public ObservableCollection<CarDetailViewModel> Items { get; set; }

        public SeznamAut()
        {
            InitializeComponent();
            Items = new ObservableCollection<CarDetailViewModel>
            {
                new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                 new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                  new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    },
                   new CarDetailViewModel
                    {
                      Title = "Skoda Octavia 3",
                      CarType = "SUV",
                      Color = "Modra",
                      Rz = "1AX 1234"
                    }
            };

            listAut.ItemsSource = Items;
        }
            
        private async void OnSwipeItemInvoked(object sender, EventArgs e)
        {
            // Navigate to a new blank page
            await Navigation.PushAsync(new DetailAuto());
        }
    }
}
