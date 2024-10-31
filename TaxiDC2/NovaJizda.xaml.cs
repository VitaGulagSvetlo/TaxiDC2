using Syncfusion.Maui.Data;
using System;
using System.Globalization;
using TaxiDC2.Tridy;

namespace TaxiDC2
{
    public partial class NovaJizda : ContentPage
    {
        public NovaJizda()
        {
            InitializeComponent();
        }

        // Method to show the date picker popup
        private void Show_DatePick(object sender, EventArgs e)
        {
            popupLayout.Show(); // Show the popup with the date picker
        }

        // This method is called when the "OK" button in the popup is clicked
        private void OnDateSelected(object sender, EventArgs e)
        {
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now;
            // Do something with the selected date (e.g., save it to a variable or display it in an Entry)
            DisplayAlert("Datum vybráno", $"Vybrané datum: {selectedDate.ToShortDateString()}", "OK");
            popupLayout.Dismiss(); // Close the popup
        }

        // Method called when "ULOŽIT" button is clicked
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

            // Create a new instance of the ride
            Jizda newRide = new Jizda
            {
                Phone = phoneNumber,
                Name = customerName,
                From = fromLocation,
                To = toLocation,
                Status = "Aktivní",
                Rating = rating,
                Note = note
            };

            App.Rides.Add(newRide);
            DisplayAlert("Úspìch", "Jízda byla úspìšnì pøidána.", "OK");
            Navigation.PopAsync();
        }
    }
}
