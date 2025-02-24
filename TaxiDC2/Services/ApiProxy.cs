using System.Net;
using Newtonsoft.Json;
using System.Text;
using TaxiDC2.Interfaces;

namespace TaxiDC2.Services
{
    public class ApiProxy : IApiProxy
    {
	    private readonly IBussinessState _bs;


	    public ApiProxy(IBussinessState bs)
        {
	        _bs = bs;
	        //bypas SSL cert check
            Client = new HttpClient();
			ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
			Client.BaseAddress =new Uri(_bs.ServerUrl);
			Client.Timeout = TimeSpan.FromSeconds(5);
        }

        private HttpClient Client { get; }

        public string BaseUrl => _bs.ServerUrl.ToLower().Replace("api/", "");

        /// <summary>
        /// Ping na server
        /// </summary>
        /// <returns>cas serveru</returns>
        public async Task<bool> Ping()
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}base/ping");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<DateTime?> res = JsonConvert.DeserializeObject<ServiceResult<DateTime?>>(s);
                    if (res != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"PING : {res.State == ResultCode.INFO}");
                        return res.State == ResultCode.INFO;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            System.Diagnostics.Debug.WriteLine($"PING : false");
            return false;
        }


        /// <summary>
        /// Vraci verzi serveru
        /// </summary>
        /// <returns>verze serveru</returns>
        public async Task<ServiceResult<string>> ServerVersion()
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}base/version");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<string> res = JsonConvert.DeserializeObject<ServiceResult<string>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<string>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<string>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Minimalni potrebna verze klienta
        /// </summary>
        /// <returns>verze req klienta</returns>
        public async Task<ServiceResult<string>> ClientMinVersion()
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}base/clientminversion");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<string> res = JsonConvert.DeserializeObject<ServiceResult<string>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<string>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<string>(ResultCode.ERROR, null, ex.Message);
            }
        }

        public async Task<ServiceResult> ForwardTrip(Guid trip, Guid driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"trips/forward/{trip}/{driver}", null);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult res = JsonConvert.DeserializeObject<ServiceResult>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }

        }

        /// <summary>
        /// Cancel cesty
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceResult> RejectTrip(Guid trip, Guid driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"trips/reject/{trip}/{driver}", null);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult res = JsonConvert.DeserializeObject<ServiceResult>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }

        }

        /// <summary>
        /// prevzeti jizdy ridicem
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceResult> AcceptByDriver(Guid trip, Guid driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"trips/accept/{trip}/{driver}", null);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult res = JsonConvert.DeserializeObject<ServiceResult>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR,message: response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message: ex.Message);
            }
        }

        /// <summary>
        /// Nacte objekt auta podle cesty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Car>> GetCarByIdAsync(Guid id)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}cars/getbyid/{id}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Car> res = JsonConvert.DeserializeObject<ServiceResult<Car>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Car>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Car>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List aut
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Car[]>> GetCarsAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}cars/{(activeOnly ? "active" : "all")}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Car[]> res = JsonConvert.DeserializeObject<ServiceResult<Car[]>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Car[]>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Car[]>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// zakaznik podle ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Customer>> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/getbyid/{id}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Customer> res = JsonConvert.DeserializeObject<ServiceResult<Customer>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Customer>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Customer>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// zakaznik podle tel. cisla
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Customer>> GetCustomerByPhone(string phone)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/getbyphone/{Base64EncodeUTF8(phone)}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Customer> res = JsonConvert.DeserializeObject<ServiceResult<Customer>>(s);
                    return res;
                }
                else
                {
                    return new ServiceResult<Customer>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Customer>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List zakazniku
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Customer[]>> GetCustomersAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/{(activeOnly ? "active" : "all")}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Customer[]> res = JsonConvert.DeserializeObject<ServiceResult<Customer[]>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Customer[]>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Customer[]>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Zakaznici podle filtru
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Customer[]>> FindCustomersAsync(string filter)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/find/{Base64EncodeUTF8(filter)}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Customer[]> res = JsonConvert.DeserializeObject<ServiceResult<Customer[]>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Customer[]>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Customer[]>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Prirazeny ridic podle ID zarizeni
        /// </summary>
        /// <param name="deviceKey"></param>
        /// <param name="deviceHash"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Driver>> GetDriverByDeviceKeyAsync(string deviceKey, string deviceHash)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}drivers/getbydevicekey/{deviceKey}/{deviceHash}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Driver> res = JsonConvert.DeserializeObject<ServiceResult<Driver>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Driver>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Driver>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// ridic podle id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Driver>> GetDriverByIdAsync(Guid id)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}drivers/getbyid/{id}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Driver> res = JsonConvert.DeserializeObject<ServiceResult<Driver>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Driver>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Driver>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List ridicu
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Driver[]>> GetDriversAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}drivers/{(activeOnly ? "active" : "all")}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Driver[]> res = JsonConvert.DeserializeObject<ServiceResult<Driver[]>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Driver[]>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Driver[]>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Cesta podle ID
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Trip>> GetTripByIdAsync(Guid tripId)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}trips/getbyid/{tripId}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                HttpResponseMessage response;
                response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Trip> res = JsonConvert.DeserializeObject<ServiceResult<Trip>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Trip>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Trip>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List cest
        /// </summary>
        /// <param name="activeOnly"> jen aktivni</param>
        /// <returns></returns>
        public async Task<ServiceResult<Trip[]>> GetTripsAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}trips/{(activeOnly ? "active" : "all")}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Trip[]> res = JsonConvert.DeserializeObject<ServiceResult<Trip[]>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Trip[]>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Trip[]>(ResultCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Meni stav cesty
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="newState"></param>
        /// <param name="paramsStrings"></param>
        /// <returns></returns>
        public async Task<ServiceResult> ChangeTripState(Guid trip, TripState newState, params string[] paramsStrings)
        {
            try
            {
                using HttpResponseMessage response =
                    await PostAsync($"trips/changestate/{trip}?ts={(int)newState}", paramsStrings);
                if (response.IsSuccessStatusCode)
                {
                    return new ServiceResult(ResultCode.OK, message:$"Zmena ulozena");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// registrace noveho ridice
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceResult> RegisterDriver(Driver driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"drivers/save", driver);
                if (response.IsSuccessStatusCode)
                {
                    return new ServiceResult(ResultCode.OK, message:$"Data ulozena");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// Ulozeni auta
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public async Task<ServiceResult> SaveCar(Car car)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"cars/save", car);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceResult(ResultCode.OK, message:$"Data ulozena");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// ulozeni zakaznika
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ServiceResult> SaveCustomer(Customer customer)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"customers/save", customer);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceResult(ResultCode.OK, message:$"Data ulozena");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// Ulozeni ridice
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceResult> SaveDriver(Driver driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"drivers/save", driver);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceResult(ResultCode.OK, message:$"Data ulozena");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// update ridice
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateDriverSetings(Driver driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"drivers/updatestate", driver);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceResult(ResultCode.OK, message:$"Data ulozena");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// update zakaznika
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateCustomerSetings(Customer customer)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"customers/updatestate", customer);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceResult(ResultCode.OK, message:$"Data ulozena");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// Ulozeni cesty
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public async Task<ServiceResult<Trip>> SaveTrip(Trip trip)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"trips/save", trip);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Trip> res = JsonConvert.DeserializeObject<ServiceResult<Trip>>(s);
                    if (res is { State: ResultCode.OK })
                        return new ServiceResult<Trip>(ResultCode.OK, res.Data, $"Data ulozena");
                    else
                        return new ServiceResult<Trip>(ResultCode.ERROR, null, "Nevratil novy objekt");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Trip>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Trip>(ResultCode.ERROR, null, ex.Message);
            }
        }

		/// <summary>
		/// Poslani notifikaci vsem aktivnim ridicum
		/// </summary>
		/// <param name="title"></param>
		/// <param name="body"></param>
		/// <param name="message"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public async Task<ServiceResult> SendNotificationToAllDevice(string title, string body, string message,
            string data)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync(
                    $"notification/sendToAllActiveDrivers?title={title}&body={body}&message={message}&data={data}", "");
                if (response.IsSuccessStatusCode)
                {
                    return new ServiceResult(ResultCode.OK, message:$"Data odeslana");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

		/// <summary>
		/// Nizkourovnovy post na API
		/// </summary>
		/// <param name="url"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		private async Task<HttpResponseMessage> PostAsync(string url, object param)
        {
            Uri uri = new($"{_bs.ServerUrl}{url}");
            System.Diagnostics.Debug.WriteLine($"[API POST] {uri}");
            string json = JsonConvert.SerializeObject(param ?? "EMPTY");
            StringContent content = new(json, Encoding.UTF8, "application/json");

            Client.DefaultRequestHeaders.Remove("USER_ID");
            Client.DefaultRequestHeaders.Add("USER_ID", _bs.DriverId?.ToString() ?? Guid.Empty.ToString());
            Client.DefaultRequestHeaders.Remove("USER_NAME");
            Client.DefaultRequestHeaders.Add("USER_NAME", _bs.Driver?.FullName ?? "NoRegUser");
            Client.DefaultRequestHeaders.Remove("CLIENT_ID");
            Client.DefaultRequestHeaders.Add("CLIENT_ID", DeviceInfo.Name);
            Client.DefaultRequestHeaders.Remove("CLIENT_VER");
            Client.DefaultRequestHeaders.Add("CLIENT_VER", VersionTracking.CurrentVersion);

            HttpResponseMessage response = await Client.PostAsync(uri, content);
            System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
            return response;
        }

        public async Task<ServiceResult> DeleteCustomerByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "customers");
        }

        public async Task<ServiceResult> DeleteTripByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "trips");
        }

        public async Task<ServiceResult> DeleteDriverByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "drivers");
        }

        public async Task<ServiceResult> DeleteCarByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "cars");
        }

        public async Task<ServiceResult> DeleteByIdAsync(Guid id, string path)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}{path}/delete/{id}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult res = JsonConvert.DeserializeObject<ServiceResult>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult(ResultCode.ERROR, message:response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult(ResultCode.ERROR, message:ex.Message);
            }
        }

        public async Task<ServiceResult<Lokace[]>> Geocode(String text)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}addr/geo/{text}");
                System.Diagnostics.Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceResult<Lokace[]> res = JsonConvert.DeserializeObject<ServiceResult<Lokace[]>>(s);
                    return res;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
                    return new ServiceResult<Lokace[]>(ResultCode.ERROR, null, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new ServiceResult<Lokace[]>(ResultCode.ERROR, null, ex.Message);
            }
        }

        public static string Base64EncodeUTF8(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        
        public static string Base64DecoceUTF8(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}