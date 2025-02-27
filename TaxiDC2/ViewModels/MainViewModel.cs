#nullable enable
namespace TaxiDC2.ViewModels;

public class MainViewModel(IDataService dataService, IBussinessState bs) : BaseViewModel(dataService)
{
	public Driver? ActiveUser => bs.ActiveUser;
	public bool IsCarAssigned => ActiveUser?.Car != null;

	public void NotifyUi()
	{
		OnPropertyChanged(nameof(ActiveUser));
		OnPropertyChanged(nameof(IsCarAssigned));
	}
}