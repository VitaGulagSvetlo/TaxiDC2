using Syncfusion.Maui.PullToRefresh;
namespace TaxiDC2;
    public partial class PullToRefreshFeatures : ContentPage
    {
        Random random = new Random();
        string[] temperatures = new string[] { "22°C", "18°C", "12°C", "25°C", "23°C", "20°C", "25°C" };
        public PullToRefreshFeatures()
        {
            InitializeComponent();
            this.BindingContext = this;
            lblTemp.Text = temperatures[0].ToString();
            pullToRefresh.Refreshing += PullToRefresh_Refreshing;
        }
        private async void PullToRefresh_Refreshing(object sender, EventArgs args)
        {
            pullToRefresh.IsRefreshing = true;
            await Task.Delay(2000);
            int number = random.Next(0, 6);
            lblTemp.Text = temperatures[number].ToString();
            pullToRefresh.IsRefreshing = false;
        }
    }
