using System.Text;

namespace TaxiDC2.Code
{
	/// <summary>
	/// Pomocne metody
	/// </summary>
	public class Utils
    {
		/// <summary>
		/// Je pripojen debuger?
		/// </summary>
		public static bool IsDebug { get { return System.Diagnostics.Debugger.IsAttached; } }

		/// <summary>
		/// Vypocita sha256 hash ze stringu
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		internal static string GetStringSha256Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            using var sha = new System.Security.Cryptography.SHA256Managed();
            byte[] textData = Encoding.UTF8.GetBytes(text);
            byte[] hash = sha.ComputeHash(textData);
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
	}
}
