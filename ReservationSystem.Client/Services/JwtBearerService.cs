using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using ReservationSystem.Client.Dtos.Accounts;

namespace ReservationSystem.Client.Services;

public class JwtBearerService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtBearerService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HttpClient?> GetClientWithTokenAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var accessToken = httpContext.Request.Cookies["access_token"];
        var refreshToken = httpContext.Request.Cookies["refresh_token"];

        if (string.IsNullOrEmpty(accessToken))
            return null;

        var client = _clientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var testResponse = await client.GetAsync("http://localhost:5284/api/users/me");

        if (testResponse.IsSuccessStatusCode)
            return client;

        if (string.IsNullOrEmpty(refreshToken))
            return null;

        var refreshContent = new StringContent(
            JsonSerializer.Serialize(new { RefreshToken = refreshToken }),
            Encoding.UTF8,
            "application/json");

        var refreshResponse = await client.PostAsync("http://localhost:5284/api/refresh-token", refreshContent);

        if (!refreshResponse.IsSuccessStatusCode)
            return null;

        var refreshBody = await refreshResponse.Content.ReadAsStringAsync();
        var tokenResponse = JsonSerializer.Deserialize<LoginResponseDto>(refreshBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (tokenResponse == null)
            return null;

        httpContext.Response.Cookies.Append("access_token", tokenResponse.JwtToken, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });
        httpContext.Response.Cookies.Append("refresh_token", tokenResponse.RefreshToken, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Strict });

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.JwtToken);

        return client;
    }

    public string? GetUserRole()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var token = httpContext?.Request.Cookies["access_token"];
        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == "role" || c.Type == "roles");
        return roleClaim?.Value;
    }

    
}