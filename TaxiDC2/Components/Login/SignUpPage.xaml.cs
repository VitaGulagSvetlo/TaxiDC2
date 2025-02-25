using TaxiDC2.ViewModels;

namespace TaxiDC2.Components.Login;

public partial class SignUpPage : ContentPage
{
	private readonly SignUpViewModel _model;

	public SignUpPage(SignUpViewModel signInViewModel)
	{
		InitializeComponent();
		BindingContext = _model = signInViewModel;
	}
}