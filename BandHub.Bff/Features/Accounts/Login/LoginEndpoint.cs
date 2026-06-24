using Microsoft.AspNetCore.Mvc;

namespace BandHub.Bff.Features.Accounts.Login;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapRegisterLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/login", async (
            [FromBody] LoginRequest request,
            LoginHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Ok(response);
            }
            catch (HttpRequestException ex)
            {
                var statusCode = (int?)ex.StatusCode ?? 500;
                return Results.Json(new { message = ex.Message }, statusCode: statusCode);
            }
        })
        .WithName("Login")
        .WithTags("Accounts");

        return app;
    }
}
