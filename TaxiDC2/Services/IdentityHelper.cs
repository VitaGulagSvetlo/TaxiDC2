using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace TaxiDC2.Services;

public interface IIdentityHelper
{
	Task<UserInfo> GetUserInfoFromIdTokenAsync();
	Task<UserInfo> GetUserInfoFromEndpointAsync();
}

public class IdentityHelper : IIdentityHelper
{
	public async Task<UserInfo> GetUserInfoFromIdTokenAsync()
	{
		var idToken = await SecureStorage.GetAsync("id_token");
		if (string.IsNullOrEmpty(idToken))
			return null;

		var handler = new JwtSecurityTokenHandler();
		var jwtToken = handler.ReadJwtToken(idToken);

		

		// JWT token má v "Claims" jednotlivé údaje
		var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
		var subject = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value; // user ID
		var name = jwtToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
		var validto = jwtToken.ValidTo;
		// Vraťte si to v nějakém vlastním objektu:
		return new UserInfo
		{
			Email = email,
			UserId = subject,
			FullName = name,
			ValidTo = validto
		};
	}


	public async Task<UserInfo> GetUserInfoFromEndpointAsync()
	{
		var accessToken = await SecureStorage.GetAsync("access_token");
		if (string.IsNullOrEmpty(accessToken))
			return null;

		using var httpClient = new HttpClient();

		// Přidáme Authorization hlavičku s Bearer tokenem:
		httpClient.DefaultRequestHeaders.Authorization
			= new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

		// Google endpoint:
		var userInfoUri = "https://www.googleapis.com/oauth2/v3/userinfo";

		try
		{
			var response = await httpClient.GetFromJsonAsync<GoogleUserInfo>(userInfoUri);
			if (response != null)
			{
				// Vraťte si to v nějakém vlastním formátu, nebo rovnou objekt:
				return new UserInfo
				{
					Email = response.Email,
					UserId = response.Sub,  // 'sub' je u Googlu unikátní ID uživatele
					FullName = response.Name
				};
			}
		}
		catch (Exception ex)
		{
			// ošetřete chyby
		}

		return null;
	}

}

// Data, která typicky Google vrací na endpointu https://www.googleapis.com/oauth2/v3/userinfo
public class GoogleUserInfo
{
	public string Sub { get; set; }       // User ID
	public string Name { get; set; }      // Celé jméno
	public string Given_name { get; set; }
	public string Family_name { get; set; }
	public string Picture { get; set; }
	public string Email { get; set; }
	public bool Email_verified { get; set; }
	public string Locale { get; set; }
}

public class UserInfo
{
	public string Email { get; set; }
	public string UserId { get; set; }
	public string FullName { get; set; }
	public DateTime ValidTo { get; set; }
}

