namespace TaxiDC2.Services
{
    public interface IBussinessState
    {
        string DeviceHash { get; }
        string DeviceKey { get; set; }
        Driver Driver { get; }
        Guid? DriverId { get; }
        public bool IsLogged { get; }
        public bool IsActive { get; }
        public bool IsAdmin { get; }
        string ServerUrl { get; set; }
        public bool TripFilter { get; set; }

        void ReloadDriver ();
        void UpdateDeviceKey(string eToken);
    }
}
