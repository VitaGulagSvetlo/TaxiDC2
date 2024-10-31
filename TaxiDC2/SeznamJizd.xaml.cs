using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using TaxiDC2.Tridy;
namespace TaxiDC2
{
    public partial class SeznamJizd : ContentPage
    {
        public ObservableCollection<Jizda> Items { get; set; }

        public SeznamJizd()
        {
            InitializeComponent();

            Items = new ObservableCollection<Jizda>
            {
                new Jizda
                {
                    Phone = "55",
                    Name = "ff",
                    From = "ff",
                    To = "dwad",
                    Date = DateTime.Now,
                    Status = "PH",
                    BackgroundColor = Color.FromHex("#FFD700"),
                    IconColor = Color.FromHex("#FFFF00"),
                    Icon = "C:\\Users\\vitsv\\source\\repos\\TaxiDC2\\TaxiDC2\\img\\car_icon.png"
                },
                new Jizda
                {
                    Phone = "4566",
                    Name = "sfh",
                    From = "fgh",
                    To = "dwadaw",
                    Date = DateTime.Now,
                    Status = "PH",
                    BackgroundColor = Color.FromHex("#FFD700"),
                    IconColor = Color.FromHex("#00FFFF"),
                    Icon = "C:\\Users\\vitsv\\source\\repos\\TaxiDC2\\TaxiDC2\\img\\user_icon.png"
                },
                
            }; listView.ItemsSource = Items;
           
            
        }
    }

    
}
