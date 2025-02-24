namespace TaxiDC2.Interfaces
{
    public interface IBussinessState
    {
	    string GoogleSUB { get; set; }

        string DeviceHash { get; }
        string DeviceKey { get; set; }
        Driver Driver { get; set; }
        Guid? DriverId { get; }
        public bool IsLogged { get; }
        public bool IsActive { get; }
        public bool IsAdmin { get; }
        string ServerUrl { get; set; }
        public bool TripFilter { get; set; }


        void ReloadDriver ();
        void UpdateDeviceKey(string eToken);

        public string AuthClient { get;  }

    }
}
