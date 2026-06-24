using BandHub.Bff.Features.Accounts.Login;
using BandHub.Bff.Features.Accounts.RegisterBand;
using BandHub.Bff.Features.Accounts.RegisterUser;
using BandHub.Bff.Integrations.BandService;
using BandHub.Bff.Integrations.UserService;

namespace BandHub.Bff.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddBff(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<UserServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:UserServiceBaseUrl"]!);
        });

        services.AddHttpClient<BandServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["Services:BandServiceBaseUrl"]!);
        });

        services.AddScoped<RegisterBandHandler>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginHandler>();

        return services;
    }
}