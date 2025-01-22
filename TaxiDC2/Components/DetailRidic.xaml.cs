using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using TaxiDC2.Models;
using TaxiDC2.ViewModels;

namespace TaxiDC2
{
    public partial class DetailRidic : ContentPage
    {
        readonly DriverViewModel _viewModel;

        public DetailRidic()
        {
            InitializeComponent();

            BindingContext = _viewModel = new DriverViewModel();
        }




        private async void OnBackButtonPressed(object sender, EventArgs e)
        {

            Shell.Current.GoToAsync($"///{nameof(SeznamRidicu)}");

        }
    }
}