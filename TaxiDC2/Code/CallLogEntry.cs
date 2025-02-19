namespace TaxiDC2
{
	public class CallLogEntry
	{
		public string PhoneNumber { get; set; }
		/// <summary>
		/// Typ hovoru: příchozí, odchozí, zmeškaný apod.
		/// Hodnoty naleznete v Android.Provider.CallLog.Calls
		/// </summary>
		public int CallType { get; set; }
		public DateTime CallDate { get; set; }
		public int Duration { get; set; }
	}
}
