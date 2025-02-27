namespace TaxiDC2.Interfaces;

/// <summary>
/// Prace s logem hovoru v telefonu
/// </summary>
public interface ICallLogService
{
	Task<List<CallLogEntry>> GetCallLogEntriesAsync();
}