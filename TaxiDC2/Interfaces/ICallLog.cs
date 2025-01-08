namespace TaxiDC2.Interfaces
{
    public interface ICallLog
    {
        IEnumerable<CallLogModel> GetCallLogs();
    }
}
