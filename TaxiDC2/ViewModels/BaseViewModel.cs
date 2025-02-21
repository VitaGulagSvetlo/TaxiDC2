using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
