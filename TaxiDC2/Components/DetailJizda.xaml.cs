using Microsoft.Maui.Controls;

namespace TaxiDC2
{
    public partial class DetailJizda : ContentPage
    {
        public DetailJizda()
        {
            InitializeComponent();
        }

        // This method will be called when the cancel button is clicked
        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to the previous page (SeznamJizd)
            await Navigation.PopAsync();
        }
    }
}
