using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Client.Dtos.Accounts;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public LoginModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [BindProperty]
    public LoginDto loginDto { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = _clientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("http://localhost:5284/api/login", loginDto);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Invalid login.");
            return Page();
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        Response.Cookies.Append("access_token", result.JwtToken, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Lax
        });

        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax
        });

        return RedirectToPage("/Index");
    }

    public class LoginResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}