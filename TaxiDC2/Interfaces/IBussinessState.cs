namespace TaxiDC2.Interfaces;

/// <summary>
/// Drzi stavove informace pro celou aplikaci
/// </summary>
public interface IBussinessState
{
	string DeviceHash { get; }
	string DeviceKey { get; set; }
	Driver? ActiveUser { get; set; }
	Guid? ActiveUserId { get; }
	public bool IsLogged { get; }
	public bool IsActive { get; }
	public bool IsAdmin { get; }
	string ServerUrl { get; set; }
	public bool TripFilter { get; set; }
	public string AuthClient { get; }
	public bool IsCarAssigned { get; }

	void UpdateDeviceKey(string eToken);
}