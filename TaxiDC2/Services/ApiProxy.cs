using System.Diagnostics;
using System.Net;
using System.Text;
using Newtonsoft.Json;

// ReSharper disable UnusedParameter.Local

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
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<DateTime?> res = JsonConvert.DeserializeObject<ServiceState<DateTime?>>(s);
                    if (res != null)
                    {
                        Debug.WriteLine($"PING : {res.State == StateCode.INFO}");
                        return res.State == StateCode.INFO;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            Debug.WriteLine("PING : false");
            return false;
        }


        /// <summary>
        /// Vraci verzi serveru
        /// </summary>
        /// <returns>verze serveru</returns>
        public async Task<ServiceState<string>> ServerVersion()
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}base/version");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<string> res = JsonConvert.DeserializeObject<ServiceState<string>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<string>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<string>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Minimalni potrebna verze klienta
        /// </summary>
        /// <returns>verze req klienta</returns>
        public async Task<ServiceState<string>> ClientMinVersion()
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}base/clientminversion");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<string> res = JsonConvert.DeserializeObject<ServiceState<string>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<string>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<string>(StateCode.ERROR, null, ex.Message);
            }
        }

        public async Task<ServiceState> ForwardTrip(Guid trip, Guid driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"trips/forward/{trip}/{driver}", null);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState res = JsonConvert.DeserializeObject<ServiceState>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }

        }

        /// <summary>
        /// Cancel cesty
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceState> RejectTrip(Guid trip, Guid driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"trips/reject/{trip}/{driver}", null);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState res = JsonConvert.DeserializeObject<ServiceState>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }

        }

        /// <summary>
        /// prevzeti jizdy ridicem
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceState> AcceptByDriver(Guid trip, Guid driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync($"trips/accept/{trip}/{driver}", null);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState res = JsonConvert.DeserializeObject<ServiceState>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR,message: response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message: ex.Message);
            }
        }

        /// <summary>
        /// Nacte objekt auta podle cesty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceState<Car>> GetCarByIdAsync(Guid id)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}cars/getbyid/{id}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Car> res = JsonConvert.DeserializeObject<ServiceState<Car>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Car>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Car>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List aut
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public async Task<ServiceState<Car[]>> GetCarsAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}cars/{(activeOnly ? "active" : "all")}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Car[]> res = JsonConvert.DeserializeObject<ServiceState<Car[]>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Car[]>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Car[]>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// zakaznik podle ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceState<Customer>> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/getbyid/{id}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Customer> res = JsonConvert.DeserializeObject<ServiceState<Customer>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Customer>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Customer>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// zakaznik podle tel. cisla
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<ServiceState<Customer>> GetCustomerByPhone(string phone)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/getbyphone/{Base64EncodeUtf8(phone)}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Customer> res = JsonConvert.DeserializeObject<ServiceState<Customer>>(s);
                    return res;
                }

                return new ServiceState<Customer>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Customer>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List zakazniku
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public async Task<ServiceState<Customer[]>> GetCustomersAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/{(activeOnly ? "active" : "all")}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Customer[]> res = JsonConvert.DeserializeObject<ServiceState<Customer[]>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Customer[]>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Customer[]>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Zakaznici podle filtru
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ServiceState<Customer[]>> FindCustomersAsync(string filter)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}customers/find/{Base64EncodeUtf8(filter)}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Customer[]> res = JsonConvert.DeserializeObject<ServiceState<Customer[]>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Customer[]>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Customer[]>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Prirazeny ridic podle ID zarizeni
        /// </summary>
        /// <param name="deviceKey"></param>
        /// <param name="deviceHash"></param>
        /// <returns></returns>
        public async Task<ServiceState<Driver>> GetDriverByDeviceKeyAsync(string deviceKey, string deviceHash)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}drivers/getbydevicekey/{deviceKey}/{deviceHash}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Driver> res = JsonConvert.DeserializeObject<ServiceState<Driver>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Driver>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Driver>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// ridic podle id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ServiceState<Driver>> GetDriverByIdAsync(Guid id)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}drivers/getbyid/{id}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Driver> res = JsonConvert.DeserializeObject<ServiceState<Driver>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Driver>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Driver>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List ridicu
        /// </summary>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public async Task<ServiceState<Driver[]>> GetDriversAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}drivers/{(activeOnly ? "active" : "all")}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Driver[]> res = JsonConvert.DeserializeObject<ServiceState<Driver[]>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Driver[]>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Driver[]>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Cesta podle ID
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public async Task<ServiceState<Trip>> GetTripByIdAsync(Guid tripId)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}trips/getbyid/{tripId}");
                Debug.WriteLine($"[API GET] {uri}");
                HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Trip> res = JsonConvert.DeserializeObject<ServiceState<Trip>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Trip>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Trip>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// List cest
        /// </summary>
        /// <param name="activeOnly"> jen aktivni</param>
        /// <returns></returns>
        public async Task<ServiceState<Trip[]>> GetTripsAsync(bool activeOnly = true)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}trips/{(activeOnly ? "active" : "all")}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Trip[]> res = JsonConvert.DeserializeObject<ServiceState<Trip[]>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Trip[]>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Trip[]>(StateCode.ERROR, null, ex.Message);
            }
        }

        /// <summary>
        /// Meni stav cesty
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="newState"></param>
        /// <param name="paramsStrings"></param>
        /// <returns></returns>
        public async Task<ServiceState> ChangeTripState(Guid trip, TripState newState, params string[] paramsStrings)
        {
            try
            {
                using HttpResponseMessage response =
                    await PostAsync($"trips/changestate/{trip}?ts={(int)newState}", paramsStrings);
                if (response.IsSuccessStatusCode)
                {
                    return new ServiceState(StateCode.OK, message:"Zmena ulozena");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// registrace noveho ridice
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceState> RegisterDriver(Driver driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync("drivers/save", driver);
                if (response.IsSuccessStatusCode)
                {
                    return new ServiceState(StateCode.OK, message:"Data ulozena");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// Ulozeni auta
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public async Task<ServiceState> SaveCar(Car car)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync("cars/save", car);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceState(StateCode.OK, message:"Data ulozena");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// ulozeni zakaznika
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ServiceState> SaveCustomer(Customer customer)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync("customers/save", customer);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceState(StateCode.OK, message:"Data ulozena");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// Ulozeni ridice
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceState> SaveDriver(Driver driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync("drivers/save", driver);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceState(StateCode.OK, message:"Data ulozena");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// update ridice
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public async Task<ServiceState> UpdateDriverSetings(Driver driver)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync("drivers/updatestate", driver);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceState(StateCode.OK, message:"Data ulozena");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// update zakaznika
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public async Task<ServiceState> UpdateCustomerSetings(Customer customer)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync("customers/updatestate", customer);
                if (response.IsSuccessStatusCode)
                {
                    //string s = await response.Content.ReadAsStringAsync();
                    return new ServiceState(StateCode.OK, message:"Data ulozena");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        /// <summary>
        /// Ulozeni cesty
        /// </summary>
        /// <param name="trip"></param>
        /// <returns></returns>
        public async Task<ServiceState<Trip>> SaveTrip(Trip trip)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync("trips/save", trip);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Trip> res = JsonConvert.DeserializeObject<ServiceState<Trip>>(s);
                    if (res is { State: StateCode.OK })
                        return new ServiceState<Trip>(StateCode.OK, res.Data, "Data ulozena");
                    return new ServiceState<Trip>(StateCode.ERROR, null, "Nevratil novy objekt");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Trip>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Trip>(StateCode.ERROR, null, ex.Message);
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
		public async Task<ServiceState> SendNotificationToAllDevice(string title, string body, string message,
            string data)
        {
            try
            {
                using HttpResponseMessage response = await PostAsync(
                    $"notification/sendToAllActiveDrivers?title={title}&body={body}&message={message}&data={data}", "");
                if (response.IsSuccessStatusCode)
                {
                    return new ServiceState(StateCode.OK, message:"Data odeslana");
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
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
            Debug.WriteLine($"[API POST] {uri}");
            string json = JsonConvert.SerializeObject(param ?? "EMPTY");
            StringContent content = new(json, Encoding.UTF8, "application/json");

            Client.DefaultRequestHeaders.Remove("USER_ID");
            Client.DefaultRequestHeaders.Add("USER_ID", _bs.ActiveUserId?.ToString() ?? Guid.Empty.ToString());
            Client.DefaultRequestHeaders.Remove("USER_NAME");
            Client.DefaultRequestHeaders.Add("USER_NAME", _bs.ActiveUser?.FullName ?? "NoRegUser");
            Client.DefaultRequestHeaders.Remove("CLIENT_ID");
            Client.DefaultRequestHeaders.Add("CLIENT_ID", DeviceInfo.Name);
            Client.DefaultRequestHeaders.Remove("CLIENT_VER");
            Client.DefaultRequestHeaders.Add("CLIENT_VER", VersionTracking.CurrentVersion);

            HttpResponseMessage response = await Client.PostAsync(uri, content);
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            return response;
        }

        public async Task<ServiceState> DeleteCustomerByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "customers");
        }

        public async Task<ServiceState> DeleteTripByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "trips");
        }

        public async Task<ServiceState> DeleteDriverByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "drivers");
        }

        public async Task<ServiceState> DeleteCarByIdAsync(Guid id)
        {
            return await DeleteByIdAsync(id, "cars");
        }

        public async Task<ServiceState> DeleteByIdAsync(Guid id, string path)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}{path}/delete/{id}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState res = JsonConvert.DeserializeObject<ServiceState>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState(StateCode.ERROR, message:response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState(StateCode.ERROR, message:ex.Message);
            }
        }

        public async Task<ServiceState<Lokace[]>> Geocode(string text)
        {
            try
            {
                Uri uri = new($"{_bs.ServerUrl}addr/geo/{text}");
                Debug.WriteLine($"[API GET] {uri}");
                using HttpResponseMessage response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    ServiceState<Lokace[]> res = JsonConvert.DeserializeObject<ServiceState<Lokace[]>>(s);
                    return res;
                }

                Debug.WriteLine(await response.Content.ReadAsStringAsync());
                return new ServiceState<Lokace[]>(StateCode.ERROR, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new ServiceState<Lokace[]>(StateCode.ERROR, null, ex.Message);
            }
        }

        public static string Base64EncodeUtf8(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        
        public static string Base64DecoceUtf8(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}