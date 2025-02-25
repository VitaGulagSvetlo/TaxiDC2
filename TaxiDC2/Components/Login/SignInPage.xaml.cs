using TaxiDC2.ViewModels;

namespace TaxiDC2.Components.Login;

public partial class SignInPage : ContentPage
{
	private readonly SignInViewModel _model;

	public SignInPage(SignInViewModel model)
	{
		InitializeComponent();
		BindingContext = _model = model;
	}
}