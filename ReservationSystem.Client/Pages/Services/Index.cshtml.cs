using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Client.Dtos.Product;

public class ServicesModel : PageModel
{
    private readonly IHttpClientFactory _clientFactory;

    public ServicesModel(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public PaginatedList<ProductDto> Products { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public async Task OnGetAsync()
    {
        var client = _clientFactory.CreateClient();

        var response = await client.GetFromJsonAsync<PaginatedList<ProductDto>>(
            $"http://localhost:5284/api/products?PageSize={PageSize}&PageNumber={PageNumber}");

        if (response != null)
        {
            Products = response;
        }
    }
}

public record PaginatedList<T>
{
    public List<T> Items { get; init; } = new();
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
}
