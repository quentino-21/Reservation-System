using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Client.Services;
using System.Text;
using System.Text.Json;

namespace ReservationSystem.Client.Pages.Reservations;

public class CreateModel : PageModel
{
    private readonly JwtBearerService _jwtBearerService;

    public CreateModel(JwtBearerService jwtBearerService)
    {
        _jwtBearerService = jwtBearerService;
    }
    
    public string? UserRole { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid ProductId { get; set; }

    [BindProperty]
    public DateTime StartTime { get; set; } = DateTime.Now.AddHours(1);

    public async Task<IActionResult> OnGetAsync()
    {
        if (ProductId == Guid.Empty)
            return BadRequest("Brak ID produktu.");

        return Page();
    }
    [BindProperty]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

    [BindProperty]
    public string Time { get; set; } = "12:00";

    public List<string> AvailableTimes { get; } = Enumerable
        .Range(8, 20 - 8) // godziny 8:00–19:30
        .SelectMany(h => new[] { "00", "30" }.Select(m => $"{h:D2}:{m}"))
        .ToList();

    public async Task<IActionResult> OnPostAsync()
    {
        UserRole = _jwtBearerService.GetUserRole();

        if (UserRole != "Admin" && UserRole != "Client")
        {
            return RedirectToPage("/Account/Login");
        }
        
        if (ProductId == Guid.Empty || string.IsNullOrEmpty(Time))
        {
            ModelState.AddModelError(string.Empty, "Wypełnij wszystkie pola.");
            return Page();
        }

        if (!TimeSpan.TryParse(Time, out var timeSpan))
        {
            ModelState.AddModelError(string.Empty, "Nieprawidłowy format godziny.");
            return Page();
        }
        
        if (!TimeOnly.TryParse(Time, out var timeOnly))
        {
            ModelState.AddModelError(string.Empty, "Nieprawidłowy format godziny.");
            return Page();
        }

        var startDateTime = Date.ToDateTime(timeOnly);

        if (startDateTime <= DateTime.Now)
        {
            ModelState.AddModelError(string.Empty, "Data rezerwacji musi być w przyszłości.");
            return Page();
        }

        var body = new
        {
            productId = ProductId,
            startTime = startDateTime
        };

        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

        var client = await _jwtBearerService.GetClientWithTokenAsync();
        if (client is null)
        {
            BadRequest();
        }
        
        var response = await client.PostAsync("http://localhost:5284/api/clients/reservations", content);
        
        var id = response.Headers.Location.ToString().Split("/").Last();
        
        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Nie udało się złożyć rezerwacji.");
            return Page();
        }

        return RedirectToPage("/Reservations/Details", new { id });
        
    }

}