using System.Globalization;
using TaxiDC2.Tridy;

namespace TaxiDC2;


    public partial class NovaJizda : ContentPage
    {
        public NovaJizda()
        {
            InitializeComponent();
        }

        // Tato metoda bude vol�na po kliknut� na tla��tko "P�idat j�zdu"
        private void OnAddRideClicked(object sender, EventArgs e)
        {
        // Z�sk�n� hodnot z formul��e
            string phoneNumber = PhoneEntry.Text;
            string customerName = CustomerEntry.Text;
            string fromLocation = FromEntry.Text;
            string toLocation = ToEntry.Text;
            DateTime rideDateTime = (DateTime)RideDate.SelectedDate;
            string status = StatusPicker.SelectedItem?.ToString();
            string note = NoteEditor.Text;

            // Validace vstup� (m��ete p�idat i slo�it�j�� validaci)
            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(fromLocation) || string.IsNullOrEmpty(toLocation))
            {
                DisplayAlert("Chyba", "Vypl�te v�echna povinn� pole.", "OK");
                return;
            }

            // Vytvo�en� nov� instance j�zdy
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

            // Ulo�en� j�zdy do datab�ze nebo seznamu (p�izp�sobte sv�mu �lo�i�ti dat)
            // Nap��klad p�id�n� do seznamu:
            App.Rides.Add(newRide);

            // Zobrazen� potvrzen� a n�vrat zp�t
            DisplayAlert("�sp�ch", "J�zda byla �sp�n� p�id�na.", "OK");
            Navigation.PopAsync();
        }
    }
