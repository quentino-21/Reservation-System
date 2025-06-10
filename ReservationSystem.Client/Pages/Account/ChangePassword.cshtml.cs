using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

public class ChangePasswordModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public ChangePasswordModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [BindProperty]
    public ChangePasswordInput Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var client = _clientFactory.CreateClient();
        var token = Request.Cookies["access_token"];
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToPage("/Login");
        }
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await client.PatchAsync("http://localhost:5284/api/users/me/change-password",
            JsonContent.Create(Input));

        Console.WriteLine(response);
        
        if (!response.IsSuccessStatusCode)
        {
            StatusMessage = "Zmiana hasła nie powiodła się.";
            return Page();
        }

        StatusMessage = "Hasło zostało zmienione.";
        return RedirectToPage();
    }

    public class ChangePasswordInput
    {
        public string CurrentPassword { get; set; } = "";
        public string NewPassword { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
    }
}