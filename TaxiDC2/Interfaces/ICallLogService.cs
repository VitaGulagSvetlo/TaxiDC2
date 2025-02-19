namespace TaxiDC2.Interfaces
{
    public interface ICallLogService
    {
	    Task<List<CallLogEntry>> GetCallLogEntriesAsync();
    }
}
