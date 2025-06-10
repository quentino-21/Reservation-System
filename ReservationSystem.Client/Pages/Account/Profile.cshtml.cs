using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using ReservationSystem.Client.Dtos.User;
using ReservationSystem.Client.Services;

namespace ReservationSystem.Client.Pages.Account;

public class ProfileModel : PageModel
{
    private readonly JwtBearerService _jwtBearerService;

    public ProfileModel(IHttpClientFactory clientFactory, JwtBearerService jwtBearerService)
    {
        _jwtBearerService = jwtBearerService;
    }

    [BindProperty]
    public UserDto User { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var client = await _jwtBearerService.GetClientWithTokenAsync();
        if (client == null)
        {
            return BadRequest();
        }
        
        var response = await client.GetAsync("http://localhost:5284/api/users/me");
        Console.WriteLine(response);

        if (!response.IsSuccessStatusCode)
        {
            StatusMessage = "Błąd podczas pobierania danych użytkownika.";
            return Page();
        }

        var user = await response.Content.ReadFromJsonAsync<UserDto>();
        if (user == null)
        {
            StatusMessage = "Nie udało się załadować profilu.";
            return Page();
        }

        User = user;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var client = await _jwtBearerService.GetClientWithTokenAsync();
        if (client == null)
        {
            return BadRequest();
        }
        var response = await client.PutAsJsonAsync("http://localhost:5284/api/users/me", new
        {
            name = User.Name,
            surname = User.Surname,
            email = User.Email
        });

        if (!response.IsSuccessStatusCode)
        {
            StatusMessage = "Aktualizacja danych nie powiodła się.";
            return Page();
        }

        StatusMessage = "Dane zostały zaktualizowane.";
        return RedirectToPage();
    }
}