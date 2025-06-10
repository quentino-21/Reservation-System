using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Client.Dtos.Reservation;
using ReservationSystem.Client.Services;

namespace ReservationSystem.Client.Pages.Reservations;

public class IndexModel : PageModel
{
    private readonly JwtBearerService _jwtBearerService;

    public IndexModel(JwtBearerService jwtBearerService)
    {
        _jwtBearerService = jwtBearerService;
    }

    public PaginatedList<ReservationDto>? Reservations { get; set; }

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;

    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var role = _jwtBearerService.GetUserRole();
        if (role != "Admin")
        {
            return Forbid();
        }

        var client = await _jwtBearerService.GetClientWithTokenAsync();
        if (client == null)
        {
            return BadRequest();
        }

        var response = await client.GetAsync($"http://localhost:5284/api/admin/reservations?PageSize={PageSize}&PageNumber={PageNumber}");
        Console.WriteLine(response);

        if (!response.IsSuccessStatusCode)
        {
            StatusMessage = "Błąd podczas pobierania rezerwacji.";
            return Page();
        }

        var reservations = await response.Content.ReadFromJsonAsync<PaginatedList<ReservationDto>>();
        if (reservations == null)
        {
            StatusMessage = "Nie udało się załadować rezerwacji.";
            return Page();
        }

        Reservations = reservations;

        return Page();
    }
}