using Hangfire;
using ReservationSystem.API.Extensions;
using ReservationSystem.Application;
using ReservationSystem.Application.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

builder.AddApplicationLogic(builder.Configuration);

builder.Services.AddJwtAuthentication();
builder.Services.AddAuthorization(options => options.AddAuthorizationPolicies());

builder.Services.AddOpenApiDocumentation();

builder.Services.ConfigureCulture();

builder.Services.AddCorsPolicy();

builder.Services.AddHangfire(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseExceptionHandler();
app.UseAuthenticationProblemDetails();

app.UseCors("FrontendClient");

app.UseHangfireDashboard();

app.UseHangfire();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "Reservation System API");
    });}

app.UseHttpsRedirection();

app.UseRequestLocalization();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterEndpoints();

await app.SeedAsync(builder.Configuration);

app.Run();