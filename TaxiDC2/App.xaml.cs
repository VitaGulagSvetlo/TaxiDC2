using TaxiDC2.Models;
namespace TaxiDC2
{
    public partial class App : Application
    {
        public App( IServiceProvider serviceProvider)
        {
            //Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUwODQwNkAzMjM3MmUzMDJlMzBjOTNIOVE2aDUwcHNsMHp0UGQzdWd3YXFlTzRLWmlJRkVuWk9OWSsvZkYwPQ==");
            InitializeComponent();

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }
    }
}
