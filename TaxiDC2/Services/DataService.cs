using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDC2.Models;

namespace TaxiDC2.Services
{
	internal interface IDataService
	{
		ObservableCollection<Trip> GetTrips();
		Task<ServiceResult<Trip>> AddTrip(Trip trip);
		Task<ServiceResult<Trip>> UpdateTrip(Trip trip);
		Task<ServiceResult<Trip>> DeleteTrip(Trip trip);

		ObservableCollection<Driver> GetDrivers();
		Task<ServiceResult<Driver>> AddDriver(Driver driver);
		Task<ServiceResult<Driver>>  UpdateDriver(Driver driver);
		Task<ServiceResult<Driver>>  DeleteDriver(Driver driver);

		ObservableCollection<Car> GetCars();
		Task<ServiceResult<Car>> AddCar(Car car);
		Task<ServiceResult<Car>> UpdateCar(Car car);
		Task<ServiceResult<Car>> DeleteCar(Car car);

		ObservableCollection<Log> GetLogs();
		Task<ServiceResult<Log>> AddLog(Log log);

		ObservableCollection<Customer> GetCustomers();
		Task<ServiceResult<Customer>> AddCustomer(Customer customer);
		Task<ServiceResult<Customer>> UpdateCustomer(Customer customer);
		Task<ServiceResult<Customer>> DeleteCustomer(Customer customer);

	}

	internal class DataService : IDataService
	{
		public ObservableCollection<Trip> GetTrips()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Trip>> AddTrip(Trip trip)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Trip>> UpdateTrip(Trip trip)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Trip>> DeleteTrip(Trip trip)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<Driver> GetDrivers()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Driver>> AddDriver(Driver driver)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Driver>> UpdateDriver(Driver driver)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Driver>> DeleteDriver(Driver driver)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<Car> GetCars()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Car>> AddCar(Car car)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Car>> UpdateCar(Car car)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Car>> DeleteCar(Car car)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<Log> GetLogs()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Log>> AddLog(Log log)
		{
			throw new NotImplementedException();
		}

		public ObservableCollection<Customer> GetCustomers()
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Customer>> AddCustomer(Customer customer)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Customer>> UpdateCustomer(Customer customer)
		{
			throw new NotImplementedException();
		}

		public async Task<ServiceResult<Customer>> DeleteCustomer(Customer customer)
		{
			throw new NotImplementedException();
		}
	}

}
