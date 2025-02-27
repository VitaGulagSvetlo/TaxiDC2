namespace TaxiDC2.Interfaces;

/// <summary>
/// Rozhrani pro praci s daty
/// </summary>
public interface IDataService
{
	Task<Car> GetCarByIdAsync(Guid carId);
	Task<Customer> GetCustomerByIdAsync(Guid customerId);
	Task<Customer> GetCustomerByPhoneAsync(string phone);
	Task<Driver> GetDriverByDeviceKeyAsync(string deviceKey, string deviceHash);
	Task<Driver> GetDriverByIdAsync(Guid driverId);
	Task<Trip> GetTripByIdAsync(Guid tripId);
	Task<bool> ChangeTripStateAsync(Guid tripId, TripState newState, params string[] paramsStrings);
	Task<bool> RegisterDriverAsync(Driver driver);
	Task<bool> SaveCarAsync(Car car);
	Task<bool> SaveDriverAsync(Driver driver);
	Task<bool> SaveCustomerAsync(Customer customer);
	Task<Trip> SaveTripAsync(Trip trip);
	Task<bool> AcceptTripByDriverAsync(Guid tripId, Guid driverId);
	Task<bool> UpdateDriverSettingsAsync(Driver driver);
	Task<bool> UpdateCustomerSettingsAsync(Customer customer);
	Task<Lokace[]> GeocodeAsync(string text);
	Task<bool> PingAsync();
	Task<string> GetServerVersionAsync();
	Task<string> GetClientMinVersionAsync();
	Task<bool> ForwardTripAsync(Guid tripId, Guid driverId);
	Task<bool> RejectTripAsync(Guid tripId, Guid driverId);
	Task<Customer[]> FindCustomersAsync(string filter);

	//seznamy
	Task<Trip[]> GetTripAsync(bool b);
	Task<Car[]> GetCarsAsync(bool activeOnly = true);
	Task<Customer[]> GetCustomersAsync(bool activeOnly = true);
	Task<Driver[]> GetDriversAsync(bool activeOnly = true);

}