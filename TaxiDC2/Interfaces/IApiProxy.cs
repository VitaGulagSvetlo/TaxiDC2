namespace TaxiDC2.Interfaces;

/// <summary>
/// Rozhrani pro praci se serverovou API
/// </summary>
public interface IApiProxy
{
	string BaseUrl { get; }

	Task<ServiceState<Car>> GetCarByIdAsync(Guid carId);

	Task<ServiceState<Car[]>> GetCarsAsync(bool activeOnly = true);

	Task<ServiceState<Customer>> GetCustomerByIdAsync(Guid customerId);

	Task<ServiceState<Customer>> GetCustomerByPhone(string phone);

	Task<ServiceState<Customer[]>> GetCustomersAsync(bool activeOnly = true);

	Task<ServiceState<Driver>> GetDriverByDeviceKeyAsync(string deviceKey,string deviceHash);

	Task<ServiceState<Driver>> GetDriverByIdAsync(Guid driverId);

	Task<ServiceState<Driver[]>> GetDriversAsync(bool activeOnly = true);

	Task<ServiceState<Trip>> GetTripByIdAsync(Guid tripId);

	Task<ServiceState<Trip[]>> GetTripsAsync(bool activeOnly = true);
        
	Task<ServiceState> ChangeTripState(Guid trip, TripState newState, params string[] paramsStrings);

	Task<ServiceState> RegisterDriver(Driver driver);

	Task<ServiceState> SaveCar(Car car);

	Task<ServiceState> SaveDriver(Driver car);

	Task<ServiceState> SaveCustomer(Customer cust);

	Task<ServiceState<Trip>> SaveTrip(Trip trip);

	Task<ServiceState> AcceptByDriver(Guid trip, Guid driver);

	Task<ServiceState> UpdateDriverSetings(Driver driver);

	Task<ServiceState> UpdateCustomerSetings(Customer customer);

	Task<ServiceState<Lokace[]>> Geocode(string text);

	Task<bool> Ping();

	Task<ServiceState<string>> ServerVersion();

	Task<ServiceState<string>> ClientMinVersion();

	Task<ServiceState> ForwardTrip(Guid tripId, Guid drv);

	Task<ServiceState> RejectTrip(Guid trip, Guid driver);

	Task<ServiceState<Customer[]>> FindCustomersAsync(string filter);
}