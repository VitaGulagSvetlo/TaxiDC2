using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using TaxiDC2.Models;

namespace TaxiDC2
{
    public partial class DetailRidic : ContentPage
    {
        public ObservableCollection<Car> Cars { get; set; }
        public Car SelectedCar { get; set; }

        public DetailRidic()
        {
            InitializeComponent();

            // Initialize sample cars
            Cars = new ObservableCollection<Car>
            {
                new Car { IdCar = Guid.NewGuid(), CarType = "�koda Superb", Color = "modr�", Rz = "2484" },
                new Car { IdCar = Guid.NewGuid(), CarType = "�koda Superb", Color = "b�l�", Rz = "1716" },
                new Car { IdCar = Guid.NewGuid(), CarType = "�koda Superb", Color = "zelen�", Rz = "0050" },
                new Car { IdCar = Guid.NewGuid(), CarType = "�koda Superb", Color = "modr�", Rz = "4884" }
            };

            // Pre-select the green car
            SelectedCar = Cars[2];

            // Set BindingContext for data binding
            BindingContext = this;
        }
    }
}