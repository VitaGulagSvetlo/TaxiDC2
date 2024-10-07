using TaxiDC2.Tridy;

namespace TaxiDC2
{
    public partial class App : Application
    {
        public static List<Jizda> Rides { get; set; } = new List<Jizda>();
        public App()
        {
            //Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUwODQwNkAzMjM3MmUzMDJlMzBjOTNIOVE2aDUwcHNsMHp0UGQzdWd3YXFlTzRLWmlJRkVuWk9OWSsvZkYwPQ==");
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
