using System.ComponentModel;

namespace TaxiDC2.ViewModels;

public class DriverViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public Guid IdDriver { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? MobileDeviceKey { get; set; }
    public bool Active { get; set; }
    public bool NotificationEnable { get; set; }
    public Guid? AssignedCar { get; set; }
    public Car? Car { get; set; }
    public string? MobileDeviceHash { get; set; }
    public bool IsAdmin { get; set; } = false;

    public string Inicials => $"{FirstName?.Substring(0, 1)}{LastName?.Substring(0, 1)}".ToUpper();

    public void LoadData(Guid id)
    {

    }

}