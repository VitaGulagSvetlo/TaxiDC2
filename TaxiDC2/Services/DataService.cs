using System.Diagnostics;

namespace TaxiDC2.Services;

public class DataService : IDataService
{
	private readonly IApiProxy _apiProxy;

	public DataService(IApiProxy apiProxy)
	{
		_apiProxy = apiProxy ?? throw new ArgumentNullException(nameof(apiProxy));
	}

	private async Task<T?> ExecuteWithRetry<T>(Func<Task<ServiceState<T>>> apiCall)
	{
		while (true)
		{
			var result = await apiCall();
			if (result.IsSuccess)
			{
				return result.Data;
			}

			// Zobrazíme chybovou hlášku s možností Retry
			bool retry = await Shell.Current.DisplayAlert("Error", result.Message, "Retry", "Cancel");

			if (!retry)
			{
				return default; // Uživatel klikl na Cancel, vracíme null
			}

			Debug.WriteLine("Retrying API call...");
		}
	}

	private async Task<bool> ExecuteWithRetry(Func<Task<ServiceState>> apiCall)
	{
		while (true)
		{
			var result = await apiCall();
			if (result.IsSuccess)
			{
				return true;
			}

			// Zobrazíme chybovou hlášku s možností Retry
			bool retry = await Shell.Current.DisplayAlert("Error", result.Message, "Retry", "Cancel");

			if (!retry)
			{
				return false; // Uživatel klikl na Cancel
			}

			Debug.WriteLine("Retrying API call...");
		}
	}

	// Použití retry mechanizmu pro API volání

	public Task<Car?> GetCarByIdAsync(Guid carId) => ExecuteWithRetry(() => _apiProxy.GetCarByIdAsync(carId));

	public Task<Car[]> GetCarsAsync(bool activeOnly = true) =>
		ExecuteWithRetry(() => _apiProxy.GetCarsAsync(activeOnly)) ?? Task.FromResult(Array.Empty<Car>());

	public Task<Customer?> GetCustomerByIdAsync(Guid customerId) =>
		ExecuteWithRetry(() => _apiProxy.GetCustomerByIdAsync(customerId));

	public Task<Driver?> GetDriverByIdAsync(Guid customerId) =>
		ExecuteWithRetry(() => _apiProxy.GetDriverByIdAsync(customerId));

	public Task<Customer?> GetCustomerByPhoneAsync(string phone) =>
		ExecuteWithRetry(() => _apiProxy.GetCustomerByPhone(phone));

	public Task<Customer[]> GetCustomersAsync(bool activeOnly = true) =>
		ExecuteWithRetry(() => _apiProxy.GetCustomersAsync(activeOnly)) ?? Task.FromResult(Array.Empty<Customer>());

	public Task<Driver?> GetDriverByDeviceKeyAsync(string deviceKey, string deviceHash) =>
		ExecuteWithRetry(() => _apiProxy.GetDriverByDeviceKeyAsync(deviceKey, deviceHash));

	public Task<Trip?> GetTripByIdAsync(Guid tripId) =>
		ExecuteWithRetry(() => _apiProxy.GetTripByIdAsync(tripId));

	public Task<bool> ChangeTripStateAsync(Guid tripId, TripState newState, params string[] paramsStrings) =>
		ExecuteWithRetry(() => _apiProxy.ChangeTripState(tripId, newState, paramsStrings));

	public Task<bool> RegisterDriverAsync(Driver driver) =>
		ExecuteWithRetry(() => _apiProxy.RegisterDriver(driver));

	public Task<bool> SaveCarAsync(Car car) =>
		ExecuteWithRetry(() => _apiProxy.SaveCar(car));

	public Task<bool> SaveDriverAsync(Driver driver) =>
		ExecuteWithRetry(() => _apiProxy.SaveDriver(driver));

	public Task<bool> SaveCustomerAsync(Customer customer) =>
		ExecuteWithRetry(() => _apiProxy.SaveCustomer(customer));

	public Task<Trip?> SaveTripAsync(Trip trip) =>
		ExecuteWithRetry(() => _apiProxy.SaveTrip(trip));

	public Task<bool> AcceptTripByDriverAsync(Guid tripId, Guid driverId) =>
		ExecuteWithRetry(() => _apiProxy.AcceptByDriver(tripId, driverId));

	public Task<bool> UpdateDriverSettingsAsync(Driver driver) =>
		ExecuteWithRetry(() => _apiProxy.UpdateDriverSetings(driver));

	public Task<bool> UpdateCustomerSettingsAsync(Customer customer) =>
		ExecuteWithRetry(() => _apiProxy.UpdateCustomerSetings(customer));

	public Task<Lokace[]> GeocodeAsync(string text) =>
		ExecuteWithRetry(() => _apiProxy.Geocode(text)) ?? Task.FromResult(Array.Empty<Lokace>());

	public Task<Trip[]> GetTripAsync(bool b) =>
		ExecuteWithRetry(() => _apiProxy.GetTripsAsync(b)) ?? Task.FromResult(Array.Empty<Trip>());

	public Task<Driver[]> GetDriversAsync(bool b) =>
		ExecuteWithRetry(() => _apiProxy.GetDriversAsync(b)) ?? Task.FromResult(Array.Empty<Driver>());

	public Task<bool> PingAsync() => _apiProxy.Ping(); // Ping nepotřebuje retry

	public Task<string?> GetServerVersionAsync() =>
		ExecuteWithRetry(() => _apiProxy.ServerVersion());

	public Task<string?> GetClientMinVersionAsync() =>
		ExecuteWithRetry(() => _apiProxy.ClientMinVersion());

	public Task<bool> ForwardTripAsync(Guid tripId, Guid driverId) =>
		ExecuteWithRetry(() => _apiProxy.ForwardTrip(tripId, driverId));

	public Task<bool> RejectTripAsync(Guid tripId, Guid driverId) =>
		ExecuteWithRetry(() => _apiProxy.RejectTrip(tripId, driverId));

	public Task<Customer[]> FindCustomersAsync(string filter) =>
		ExecuteWithRetry(() => _apiProxy.FindCustomersAsync(filter)) ?? Task.FromResult(Array.Empty<Customer>());
}