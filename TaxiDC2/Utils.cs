using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiDC2
{
    public class Utils
    {
        public static bool IsDebug {  get { return System.Diagnostics.Debugger.IsAttached; } }


        internal static string GetStringSha256Hash(string text)
        {
	        if (String.IsNullOrEmpty(text))
		        return String.Empty;

	        using (var sha = new System.Security.Cryptography.SHA256Managed())
	        {
		        byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
		        byte[] hash = sha.ComputeHash(textData);
		        return BitConverter.ToString(hash).Replace("-", String.Empty);
	        }
        }
	}
}
