using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ReservationSystem.Client.Services;

namespace ReservationSystem.Client.Pages.Services;

public class DetailsModelServices : PageModel
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JwtBearerService _jwtBearerService;

    public DetailsModelServices(IHttpClientFactory clientFactory, JwtBearerService jwtBearerService)
    {
        _clientFactory = clientFactory;
        _jwtBearerService = jwtBearerService;
    }
    
    public string? UserRole { get; set; }

    public ProductDto? Product { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        UserRole = _jwtBearerService.GetUserRole();
        
        if (Id == Guid.Empty)
        {
            return NotFound();
        }

        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5284/api/products/{Id}");

        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        Product = JsonSerializer.Deserialize<ProductDto>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (Product == null)
        {
            return NotFound();
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