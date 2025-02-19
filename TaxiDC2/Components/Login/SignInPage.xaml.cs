using TaxiDC2.ViewModels;

namespace TaxiDC2.Components.Login;

public partial class SignInPage : ContentPage
{
	private readonly SignInViewModel _signInViewModel;

	public SignInPage(SignInViewModel signInViewModel)
	{
		InitializeComponent();
		BindingContext = _signInViewModel = signInViewModel;
	}
}