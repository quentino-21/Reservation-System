using ReservationSystem.Client.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient("api", c =>
    c.BaseAddress = new Uri("http://localhost:5284/api/"));

builder.Services.AddScoped<JwtBearerService>();


var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseRouting();

app.MapRazorPages();

app.Run();