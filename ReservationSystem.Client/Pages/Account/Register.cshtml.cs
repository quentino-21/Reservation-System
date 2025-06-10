using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

public class RegisterModel : PageModel
{
    [BindProperty]
    public RegisterInput Input { get; set; } = new();

    private readonly IHttpClientFactory _clientFactory;

    public RegisterModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var client = _clientFactory.CreateClient();
        var json = JsonSerializer.Serialize(Input);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("http://localhost:5284/api/register", content);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Rejestracja nie powiodła się.");
            return Page();
        }

        return RedirectToPage("/Index");
    }

    public class RegisterInput
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string Role { get; set; } = "Client";
    }
}
