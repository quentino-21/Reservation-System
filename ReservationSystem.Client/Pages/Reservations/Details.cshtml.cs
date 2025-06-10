using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using ReservationSystem.Client.Dtos.Reservation;
using ReservationSystem.Client.Services;

namespace ReservationSystem.Client.Pages.Reservations;

public class DetailsModelReservations : PageModel
{
    private readonly JwtBearerService _jwtBearerService;
    
    public string? UserRole { get; set; }


    public DetailsModelReservations(JwtBearerService jwtBearerService)
    {
        _jwtBearerService = jwtBearerService;
    }
    
    public async Task<IActionResult> OnPostConfirmAsync()
    {
        return await HandleStatusChangeAsync("confirm");
    }

    public async Task<IActionResult> OnPostRejectAsync()
    {
        return await HandleStatusChangeAsync("reject");
    }

    public async Task<IActionResult> OnPostCompleteAsync()
    {
        return await HandleStatusChangeAsync("complete");
    }

    public async Task<IActionResult> OnPostCancelAsync()
    {
        return await HandleStatusChangeAsync("cancel");
    }

    private async Task<IActionResult> HandleStatusChangeAsync(string action)
    {
        UserRole = _jwtBearerService.GetUserRole();
    
        if (Id == Guid.Empty)
        {
            return NotFound();
        }

        var client = await _jwtBearerService.GetClientWithTokenAsync();
        if (client == null)
        {
            return BadRequest();
        }

        var url = UserRole == "Admin"
            ? $"http://localhost:5284/api/admin/reservations/{Id}/{action}"
            : $"http://localhost:5284/api/clients/reservations/{Id}/{action}";

        var response = await client.PostAsync(url, null);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(errorContent);
            string? errorMessage = null;

            try
            {
                using var doc = JsonDocument.Parse(errorContent);
                if (doc.RootElement.TryGetProperty("detail", out var msg))
                {
                    errorMessage = msg.GetString();
                }
            }
            catch
            {
                errorMessage = errorContent;
            }

            ModelState.AddModelError(string.Empty, $"Nie udało się wykonać akcji: {errorMessage ?? "Błąd serwera."}");
            return Page();
        }

        return RedirectToPage(new { id = Id });
    }

    public ReservationDto? Reservation { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }
        
        

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == Guid.Empty)
        {
            return NotFound();
        }
    
        UserRole = _jwtBearerService.GetUserRole();

        var client = await _jwtBearerService.GetClientWithTokenAsync();
        if (client == null)
        {
            return BadRequest();
        }
    
        var url = UserRole == "Admin"
            ? $"http://localhost:5284/api/admin/reservations/{Id}"
            : $"http://localhost:5284/api/clients/reservations/{Id}";
    
        var response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            string? errorMessage = null;

            try
            {
                using var doc = JsonDocument.Parse(errorContent);
                if (doc.RootElement.TryGetProperty("message", out var msg))
                {
                    errorMessage = msg.GetString();
                }
            }
            catch
            {
                errorMessage = errorContent;
            }

            ModelState.AddModelError(string.Empty, errorMessage ?? "Wystąpił błąd podczas pobierania rezerwacji.");
            return Page();
        }

        var json = await response.Content.ReadAsStringAsync();
        Reservation = JsonSerializer.Deserialize<ReservationDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (Reservation == null)
        {
            ModelState.AddModelError(string.Empty, "Nie znaleziono rezerwacji.");
            return Page();
        }

        return Page();
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        
        public string PriceFormatted => Price.ToString("C", System.Globalization.CultureInfo.GetCultureInfo("pl-PL"));
    }
}