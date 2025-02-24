namespace TaxiDC2.Interfaces
{
    public interface IApiProxy
    {
        string BaseUrl { get; }

        Task<ServiceResult<Car>> GetCarByIdAsync(Guid carId);

        Task<ServiceResult<Car[]>> GetCarsAsync(bool activeOnly = true);

        Task<ServiceResult<Customer>> GetCustomerByIdAsync(Guid customerId);

        Task<ServiceResult<Customer>> GetCustomerByPhone(string phone);

        Task<ServiceResult<Customer[]>> GetCustomersAsync(bool activeOnly = true);

        Task<ServiceResult<Driver>> GetDriverByDeviceKeyAsync(string deviceKey,string deviceHash);

        Task<ServiceResult<Driver>> GetDriverByIdAsync(Guid driverId);

        Task<ServiceResult<Driver[]>> GetDriversAsync(bool activeOnly = true);

        Task<ServiceResult<Trip>> GetTripByIdAsync(Guid tripId);

        Task<ServiceResult<Trip[]>> GetTripsAsync(bool activeOnly = true);
        
        Task<ServiceResult> ChangeTripState(Guid trip, TripState newState, params string[] paramsStrings);

        Task<ServiceResult> RegisterDriver(Driver driver);

        Task<ServiceResult> SaveCar(Car car);

        Task<ServiceResult> SaveDriver(Driver car);

        Task<ServiceResult> SaveCustomer(Customer cust);

        Task<ServiceResult<Trip>> SaveTrip(Trip trip);

        Task<ServiceResult> AcceptByDriver(Guid trip, Guid driver);

        Task<ServiceResult> UpdateDriverSetings(Driver driver);

        Task<ServiceResult> UpdateCustomerSetings(Customer customer);

        Task<ServiceResult<Lokace[]>> Geocode(String text);

        Task<bool> Ping();

        Task<ServiceResult<string>> ServerVersion();

        Task<ServiceResult<string>> ClientMinVersion();

        Task<ServiceResult> ForwardTrip(Guid tripId, Guid drv);

        Task<ServiceResult> RejectTrip(Guid trip, Guid driver);

        Task<ServiceResult<Customer[]>> FindCustomersAsync(string filter);
    }
}