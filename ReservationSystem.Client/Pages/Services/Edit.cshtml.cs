using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Client.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace ReservationSystem.Client.Pages.Services;

public class EditModelServices : PageModel
{
    private readonly JwtBearerService _jwtBearerService;

    public EditModelServices(JwtBearerService jwtBearerService)
    {
        _jwtBearerService = jwtBearerService;
    }

    [BindProperty]
    public ProductDto Product { get; set; } = new();

    public string? StatusMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Id == Guid.Empty)
        {
            StatusMessage = "Nieprawidłowy identyfikator usługi.";
            return Page();
        }

        var client = await _jwtBearerService.GetClientWithTokenAsync();
        var response = await client.GetAsync($"http://localhost:5284/api/products/{Id}");

        if (!response.IsSuccessStatusCode)
        {
            StatusMessage = "Nie udało się pobrać danych usługi.";
            return Page();
        }

        var product = await response.Content.ReadFromJsonAsync<ProductDto>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (product == null)
        {
            StatusMessage = "Usługa nie istnieje.";
            return Page();
        }

        Product = product;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            StatusMessage = "Formularz zawiera błędy.";
            return Page();
        }

        var client = await _jwtBearerService.GetClientWithTokenAsync();

        Console.WriteLine($"Product.Id: {Product.Id}");

        var response = await client.PutAsJsonAsync(
            $"http://localhost:5284/api/admin/products/{Product.Id}",
            Product
        );

        if (!response.IsSuccessStatusCode)
        {
            StatusMessage = "Nie udało się zaktualizować usługi.";
            return Page();
        }

        return RedirectToPage("/Services/Details", new { id = Product.Id });
    }

    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
    }
}
