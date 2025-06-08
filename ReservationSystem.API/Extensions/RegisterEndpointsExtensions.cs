using ReservationSystem.API.Endpoints;

namespace ReservationSystem.API.Extensions;

public static class RegisterEndpointsExtensions
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder app)
    {
        app
            .MapAccountsApi()
            .MapUserApi()
            .MapProductApi()
            .MapAdminApi()
            .MapClientApi();
    }
}