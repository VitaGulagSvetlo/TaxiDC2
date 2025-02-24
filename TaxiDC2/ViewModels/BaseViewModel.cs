using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxiDC2.ViewModels
{
	[ObservableObject]
	public partial class BaseViewModel(IDataService dataService)
	{
		protected IDataService DataService { get; } = dataService ?? throw new ArgumentNullException(nameof(dataService));

		[ObservableProperty]
		private bool isBusy = false;
		[ObservableProperty]
		private string title = string.Empty;
		[ObservableProperty]
		private string message = string.Empty;

	}
}
