using System.ComponentModel;
using System.Windows.Input;

namespace TaxiDC2.ViewModels;

public class VersionModel:INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private IBussinessState _bs;
    private IApiProxy _proxy;

    public ICommand UpdateCmd { get; }


    public VersionModel(IBussinessState bs, IApiProxy proxy)
    {
        _bs = bs;
        _proxy = proxy;
        Title = "Nastaven�";
        UpdateCmd = new Command(async () => await Update());
        VersionString = VersionTracking.CurrentVersion;
        ServerVersionString = Task.Run(async()=>await _proxy.ClientMinVersion()).Result.Data;
    }
    
    private async Task Update()
    {
        try
        {
            await Browser.OpenAsync("https://api.advisor-soft.com:8015/update.html", BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            // An unexpected error occured. No browser may be installed on the device.
        }
    }

    public string Title { get; set; }

    public object VersionString { get; set; }

    public string ServerVersionString { get; set; }

    public bool VersionValid => ServerVersionString != null && VersionString != null && VersionString == ServerVersionString;
}