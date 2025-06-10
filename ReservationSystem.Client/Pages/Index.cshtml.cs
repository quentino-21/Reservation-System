using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReservationSystem.Client.Dtos.IpApi;

namespace ReservationSystem.Client.Pages;

public class IndexModel : PageModel
{
    public IpApiResponseDto? ResponseDto { get; set; }

    public async Task OnGetAsync()
    {
        using var http = new HttpClient();
        var response = await http.GetAsync("https://ipwho.is/");
        Console.WriteLine(response);
    
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<IpApiResponseDto>(json);
            Console.WriteLine(json);
            Console.WriteLine(data);
            
            ResponseDto = data;
        }
    }

}
