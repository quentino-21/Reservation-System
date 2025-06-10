using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Client.Dtos.Reservation;
using ReservationSystem.Client.Services;

namespace ReservationSystem.Client.Pages.Reservations;

public class MyReservationsModel : PageModel
{
    private readonly JwtBearerService _jwtBearerService;
    
    public MyReservationsModel(JwtBearerService jwtBearerService)
    {
        _jwtBearerService = jwtBearerService;
    }
    
    public PaginatedList<ReservationDto> Reservations { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;
    public string? StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var client = await _jwtBearerService.GetClientWithTokenAsync();
        if (client == null)
        {
            return BadRequest();
        }
        
        var response = await client.GetAsync($"http://localhost:5284/api/clients/reservations?PageSize={PageSize}&PageNumber={PageNumber}");

        if (!response.IsSuccessStatusCode)
        {
            StatusMessage = "Błąd podczas pobierania danych użytkownika.";
            return Page();
        }
        
        var reservation = await response.Content.ReadFromJsonAsync<PaginatedList<ReservationDto>>();
        if (reservation == null)
        {
            StatusMessage = "Nie udało się załadować rezerwacji.";
            return Page();
        }

        Reservations = reservation;
        return Page();
    }
}