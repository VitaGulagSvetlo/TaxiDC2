using TaxiDC2.ViewModels;

namespace TaxiDC2;

public partial class AboutPage : ContentPage
{
	public AboutPage( ConfigViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}