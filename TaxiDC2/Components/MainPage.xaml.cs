using IdentityModel.OidcClient;

namespace TaxiDC2
{
	public partial class MainPage : ContentPage
	{
		private readonly OidcClient _client = default!;
		private readonly IIdentityHelper _identityHelper;
		private readonly IBussinessState _bussinessState;
		private readonly IApiProxy _proxy;
		private string? _currentAccessToken;

		public MainPage(
			OidcClient client,
			IIdentityHelper identityHelper,
			IBussinessState bussinessState,
			IApiProxy proxy)
		{
			InitializeComponent();

			_client = client;
			_identityHelper = identityHelper;
			_bussinessState = bussinessState;
			_proxy = proxy;
		//	l1.Text = bussinessState.AuthClient;
		}

		//private async void OnLoginClicked(object sender, EventArgs e)
		//{
		//	editor.Text = "Login Clicked";

		//	var result = await _client.LoginAsync();

		//	if (result.IsError)
		//	{
		//		editor.Text = result.Error;
		//		return;
		//	}

		//	_currentAccessToken = result.AccessToken;

		//	// ulozeni tokenu
		//	await SecureStorage.SetAsync("access_token", result.AccessToken);
		//	await SecureStorage.SetAsync("id_token", result.IdentityToken);
		//	if (result.RefreshToken != null)
		//		await SecureStorage.SetAsync("refresh_token", result.RefreshToken);


		//	var sb = new StringBuilder(128);
		//	var ui = await _client.GetUserInfoAsync(_currentAccessToken, CancellationToken.None);

		//	_bussinessState.GoogleSUB = result.User.FindFirst("sub")?.Value;

		//	sb.AppendLine("claims:");
		//	foreach (var claim in result.User.Claims)
		//	{
		//		sb.AppendLine($"{claim.Type}: {claim.Value}");
		//	}

		//	sb.AppendLine();
		//	sb.AppendLine("access token:");
		//	//sb.AppendLine(result.AccessToken);

		//	if (!string.IsNullOrWhiteSpace(result.RefreshToken))
		//	{
		//		sb.AppendLine();
		//		sb.AppendLine("refresh token:");
		//		// sb.AppendLine(result.RefreshToken);
		//	}

		//	editor.Text = sb.ToString();
		//}

		//private async void OnInfoClicked(object sender, EventArgs e)
		//{
		//	var userInfo = await _identityHelper.GetUserInfoFromIdTokenAsync();
		//	if (userInfo != null)
		//	{
		//		editor.Text = JsonSerializer.Serialize(userInfo, new JsonSerializerOptions { WriteIndented = true });
		//	}
		//	else
		//	{
		//		editor.Text = "No user info found";
		//	}

		//	var r = await _proxy.ServerVersion();

		//	userInfo = await _identityHelper.GetUserInfoFromEndpointAsync();
		//	if (userInfo != null)
		//	{
		//		editor.Text = JsonSerializer.Serialize(userInfo, new JsonSerializerOptions { WriteIndented = true });
		//	}
		//	else
		//	{
		//		editor.Text = "No user info found";
		//	}
		//}

		//private async void OnApiClicked(object sender, EventArgs e)
		//{
		//	editor.Text = "API Clicked";

		//	if (_currentAccessToken != null)
		//	{
		//		var client = new HttpClient();
		//		client.SetBearerToken(_currentAccessToken);

		//		var response = await client.GetAsync("https://demo.duendesoftware.com/api/test");
		//		if (response.IsSuccessStatusCode)
		//		{
		//			var content = await response.Content.ReadAsStringAsync();
		//			var doc = JsonDocument.Parse(content).RootElement;
		//			editor.Text = JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
		//		}
		//		else
		//		{
		//			editor.Text = response.ReasonPhrase;
		//		}
		//	}
		//}

		private void OnNewClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(NovaJizda)}");
		}

		private void OnListClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(SeznamJizd)}");
		}

		private void OnSetClicked(object sender, EventArgs e)
		{
			Shell.Current.GoToAsync($"{nameof(AboutPage)}");
		}

		private async void OnBackButtonPressed(object sender, EventArgs e)
		{
			//await Shell.Current.GoToAsync($"{nameof(MainPage)}");
		}

	}

}
