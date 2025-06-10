using System.Text.Json.Serialization;

namespace ReservationSystem.Client.Dtos.IpApi;

public sealed record IpApiResponseDto
{
    [JsonPropertyName("ip")]
    public string? Ip { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("country_code")]
    public string? Country_Code { get; set; }

    [JsonPropertyName("region")]
    public string? Region { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }
}
