using System.Globalization;
using TaxiDC2.Tridy;

namespace TaxiDC2;


    public partial class NovaJizda : ContentPage
    {
        public NovaJizda()
        {
            InitializeComponent();
        }

        // Tato metoda bude volána po kliknutí na tlaèítko "Pøidat jízdu"
        private void OnAddRideClicked(object sender, EventArgs e)
        {
        // Získání hodnot z formuláøe
            string phoneNumber = PhoneEntry.Text;
            string customerName = CustomerEntry.Text;
            string fromLocation = FromEntry.Text;
            string toLocation = ToEntry.Text;
            DateTime rideDateTime = (DateTime)RideDate.SelectedDate;
            string status = StatusPicker.SelectedItem?.ToString();
            string note = NoteEditor.Text;

            // Validace vstupù (mùžete pøidat i složitìjší validaci)
            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(fromLocation) || string.IsNullOrEmpty(toLocation))
            {
                DisplayAlert("Chyba", "Vyplòte všechna povinná pole.", "OK");
                return;
            }

            // Vytvoøení nové instance jízdy
            Jizda newRide = new Jizda
            {
                Cislo = phoneNumber,
                Jmeno = customerName,
                Odkud = fromLocation,
                Kam = toLocation,
                Kdy = rideDateTime,
                Status = status,
                Poznamka = note
            };

            // Uložení jízdy do databáze nebo seznamu (pøizpùsobte svému úložišti dat)
            // Napøíklad pøidání do seznamu:
            App.Rides.Add(newRide);

            // Zobrazení potvrzení a návrat zpìt
            DisplayAlert("Úspìch", "Jízda byla úspìšnì pøidána.", "OK");
            Navigation.PopAsync();
        }
    }
