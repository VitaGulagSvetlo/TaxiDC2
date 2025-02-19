using Firebase.Auth;
using TaxiDC2.Code;

namespace TaxiDC2.Services
{
    public class BussinessState : IBussinessState
    {
	    private readonly FirebaseAuthClient _authClient;

	    public BussinessState(FirebaseAuthClient authClient)
	    {
		    _authClient = authClient;
	    }

	    public string AuthClient => _authClient?.User?.Info?.DisplayName;

        private static string _deviceKey;

        private Driver _driver = null;

        /// <summary>
        /// Klic zarizeni
        /// </summary>
        public string DeviceKey { get => _deviceKey; set => _deviceKey = value; }

        /// <summary>
        /// Je prihlasen uzivatel ?
        /// </summary>
        public bool IsLogged => Driver != null;
        
        // je uzivatel povolen ?
        public bool IsActive => _driver?.Active??false;

        public Driver Driver
        {
            get
            {
                if (_driver == null)
                    ReloadDriver();
                return _driver;
            }
        }

        /// <summary>
        /// ID aktualniho usera
        /// </summary>
        public Guid? DriverId => _driver?.IdDriver;
        
        /// <summary>
        /// je aktualni user adminem ?
        /// </summary>
        public bool IsAdmin => Driver?.IsAdmin??false;

        /// <summary>
        /// Bazova adresa API
        /// </summary>
        public string ServerUrl
        {
//	        get => Preferences.Get("ServerUrl", "https://test.advisor-soft.com:8015/api/");
	        get => Preferences.Get("ServerUrl", "https://api.advisor-soft.com:9091/api/");
	        set => Preferences.Set("ServerUrl", value);
        }

		/// <summary>
		/// Nastaveni filtru cest
		/// </summary>
		public bool TripFilter
        {
	        get => Preferences.Get("TripFilter", false);
	        set => Preferences.Set("TripFilter", value);
        }
        
		/// <summary>
		/// Nacti znovu uzivatele
		/// </summary>
		public void ReloadDriver()
        {
            if (string.IsNullOrWhiteSpace(_deviceKey)) return;

            IApiProxy proxy = DependencyService.Get<IApiProxy>();
            if (proxy == null) return;
            ServiceResult<Driver> drv = Task.Run(async () => await proxy.GetDriverByDeviceKeyAsync(DeviceKey, DeviceHash)).Result;
            if (drv.State == ResultCode.OK)
                _driver = drv.Data;
        }

        /// <summary>
        /// Aktualizace klice zarizeni pokud se zmeni
        /// </summary>
        /// <param name="eToken"></param>
        public void UpdateDeviceKey(string eToken)
        {
            if (_driver != null && _driver.MobileDeviceKey != eToken)
            {
                // todo: musi se updatovat klic v DB u uzivatele
            }
            _deviceKey = eToken;
        }

		/// <summary>
		/// Google ID uzivatele
		/// </summary>
		public string GoogleSUB { get; set; }

        /// <summary>
        /// Otisk aktualniho zarizeni
        /// </summary>
        public string DeviceHash
        {
            get
            {
                string s = $"{DeviceInfo.Name}{DeviceInfo.Manufacturer}{DeviceInfo.Model}";
                System.Diagnostics.Debug.WriteLine(s);
                return Utils.GetStringSha256Hash(s);
            }
        }
    }
}