using Syncfusion.Maui.Data;
using Syncfusion.Maui.Picker;
using Syncfusion.Maui.Popup;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;
using TaxiDC2.Models;

namespace TaxiDC2
{
    public partial class NovaJizda : ContentPage
    {
        public ObservableCollection<int> MinutesList { get; set; }
        public int SelectedMinute { get; set; }
        public NovaJizda()
        {
            InitializeComponent();
            MinutesList = new ObservableCollection<int> { 5, 10, 15, 20, 25, 30 };
            BindingContext = this;
        }
        private async void OnTimePickerButtonClicked(object sender, EventArgs e)
        {
            Syncfusion.Maui.Popup.SfPopup timePickerPopup = null;

            var timePicker = new SfTimePicker
            {
                
                Format = PickerTimeFormat.HH_mm,
                SelectedTime = TimeSpan.Zero,
                WidthRequest = 300,
                HeightRequest = 200
            };

            timePickerPopup = new Syncfusion.Maui.Popup.SfPopup
            {
                WidthRequest = 350,
                HeightRequest = 400,
                ContentTemplate = new DataTemplate(() =>
                {
                    return new VerticalStackLayout
                    {
                        Spacing = 10,
                        Children =
                {
                    timePicker,
                    new Button
                    {
                        Text = "OK",
                        BackgroundColor = Colors.Orange,
                        TextColor = Colors.White,
                        WidthRequest = 100, 
                        Command = new Command(() =>
                        {
                            string selectedTime = timePicker.SelectedTime?.ToString(@"hh\:mm") ?? "00:00";
                            TimePickerButton.Text = selectedTime; 
                            timePickerPopup.IsOpen = false; 
                        })
                    },
                    new Button
                    {
                        Text = "Zrušit",
                        BackgroundColor = Colors.Gray,
                        TextColor = Colors.White,
                        WidthRequest = 100,
                        Command = new Command(() =>
                        {
                            timePickerPopup.IsOpen = false; 
                        })
                    }
                }
                    };
                }),
                IsOpen = true 
            };

            await Task.CompletedTask;
        }

        private void OnAddRideClicked(object sender, EventArgs e)
        {
            string phoneNumber = PhoneEntry.Text;
            string customerName = NameEntry.Text;
            string fromLocation = FromEntry.Text;
            string toLocation = ToEntry.Text;
            double rating = CustomerRating.Value;
            string note = NoteEntry.Text;

            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(fromLocation) || string.IsNullOrEmpty(toLocation))
            {
                DisplayAlert("Chyba", "Vyplòte všechna povinná pole.", "OK");
                return;
            }

            Trip newRide = new Trip
            {
               
                AddressBoarding = fromLocation,
                AddressExit = toLocation,             
              
            };

            App.Rides.Add(newRide);
            DisplayAlert("Úspìch", "Jízda byla úspìšnì pøidána.", "OK");
            Navigation.PopAsync();
        }
    }
}
