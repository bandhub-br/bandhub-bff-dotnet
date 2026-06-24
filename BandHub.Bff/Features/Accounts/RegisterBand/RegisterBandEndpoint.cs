namespace BandHub.Bff.Features.Accounts.RegisterBand;

public static class RegisterBandEndpoint
{
    public static IEndpointRouteBuilder MapRegisterBandEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/accounts/register/band", async (
            RegisterBandRequest request,
            RegisterBandHandler handler,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var response = await handler.HandleAsync(request, cancellationToken);
                return Results.Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        })
        .WithName("RegisterBand")
        .WithTags("Accounts");

        return app;
    }
}