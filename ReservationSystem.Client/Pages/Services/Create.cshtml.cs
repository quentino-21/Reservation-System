using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Client.Services;
using System.Text.Json;
using System.Net.Http.Json;
using ReservationSystem.Client.Dtos.Product;

namespace ReservationSystem.Client.Pages.Services
{
    public class CreateModel : PageModel
    {
        private readonly JwtBearerService _jwtBearerService;

        public CreateModel(JwtBearerService jwtBearerService)
        {
            _jwtBearerService = jwtBearerService;
        }

        [BindProperty] public CreateProductDto NewProduct { get; set; }

        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = await _jwtBearerService.GetClientWithTokenAsync();
            if (client == null)
            {
                StatusMessage = "Brak uprawnień.";
                return Page();
            }

            var response = await client.PostAsJsonAsync("http://localhost:5284/api/admin/products", NewProduct);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                StatusMessage = $"Błąd podczas dodawania produktu: {error}";
                return Page();
            }
        }
    }
}