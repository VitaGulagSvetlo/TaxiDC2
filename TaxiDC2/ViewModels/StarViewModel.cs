using System.ComponentModel;

namespace TaxiDC2.ViewModels;

public class StarViewModel :INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public int VIP { get; set; } = 0;

    [DependsOn("VIP")]
    public bool St1Visible => VIP > 0;
    [DependsOn("VIP2")]
    public bool St2Visible => VIP > 1;
    [DependsOn("VIP2")]
    public bool St3Visible => VIP > 2;
    [DependsOn("VIP2")]
    public bool St4Visible => VIP > 3;
    [DependsOn("VIP2")]
    public bool St5Visible => VIP > 4;

    public int Size { get; set; } = 30;

    public StarViewModel()
    {
        VIP = 1;
        Size = 38;
    }

    public StarViewModel(int value,int size)
    {
        VIP = value;
        Size = size;
    }


}