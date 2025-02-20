using System.Diagnostics;
using TaxiDC2.ViewModels;

namespace TaxiDC2.Components;

public partial class LoadingPage : ContentPage
{
	private LoadingPageViewModel _model;

	public LoadingPage(LoadingPageViewModel model)
	{
		InitializeComponent();
		BindingContext = _model = model;
	}

}