using Microsoft.AspNetCore.Mvc;

namespace BandHub.Bff.Features.Accounts.RegisterUser;

public static class RegisterUserEndpoint
{
    public static IEndpointRouteBuilder MapRegisterUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register/user", async (
            [FromBody] RegisterUserRequest request,
            RegisterUserHandler handler,
            CancellationToken cancellationToken) =>
        {
            var response = await handler.HandleAsync(request, cancellationToken);
            return Results.Created($"/accounts/{response.Id}", response);
        })
        .WithName("RegisterUser")
        .WithTags("Accounts");

        return app;
    }
}
