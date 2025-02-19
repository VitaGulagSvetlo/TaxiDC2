using Android.Content.PM;
using Android.Provider;
using TaxiDC2.Interfaces;
using Application = Android.App.Application;

namespace TaxiDC2.Platforms.Android;



public class CallLogService : ICallLogService
{
	public async Task<List<CallLogEntry>> GetCallLogEntriesAsync()
	{
		var callLogs = new List<CallLogEntry>();

		// Zkontrolujte, zda je oprávnění uděleno
		if (Application.Context.CheckSelfPermission("android.permission.READ_CALL_LOG") != Permission.Granted)
		{
			// Vytvořte metodu pro asynchronní žádost o oprávnění.
			// Tuto část musíte implementovat podle vaší architektury (např. pomocí ActivityCompat.RequestPermissions)
			// Pro tento příklad předpokládáme, že oprávnění ještě nebylo uděleno.
			return callLogs;
		}

		var contentResolver = Application.Context.ContentResolver;
		var uri = CallLog.Calls.ContentUri;

		string[] projection = new string[]
		{
			CallLog.Calls.Number,
			CallLog.Calls.Type,
			CallLog.Calls.Date,
			CallLog.Calls.Duration
		};

		// Řazení od nejnovějšího
		var cursor = contentResolver.Query(uri, projection, null, null, CallLog.Calls.Date + " DESC");

		if (cursor != null)
		{
			try
			{
				if (cursor.MoveToFirst())
				{
					do
					{
						var number = cursor.GetString(cursor.GetColumnIndex(projection[0]));
						var type = cursor.GetInt(cursor.GetColumnIndex(projection[1]));
						var dateLong = cursor.GetLong(cursor.GetColumnIndex(projection[2]));
						var duration = cursor.GetInt(cursor.GetColumnIndex(projection[3]));

						callLogs.Add(new CallLogEntry
						{
							PhoneNumber = number,
							CallType = type,
							CallDate = DateTimeOffset.FromUnixTimeMilliseconds(dateLong).DateTime,
							Duration = duration
						});
					}
					while (cursor.MoveToNext());
				}
			}
			finally
			{
				cursor.Close();
			}
		}

		return callLogs;
	}
}