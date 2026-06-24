using BandHub.Bff.Integrations.BandService;
using BandHub.Bff.Integrations.UserService;

namespace BandHub.Bff.Features.Accounts.RegisterBand;

public class RegisterBandHandler
{
    private readonly UserServiceClient _userServiceClient;
    private readonly BandServiceClient _bandServiceClient;

    public RegisterBandHandler(UserServiceClient userServiceClient, BandServiceClient bandServiceClient)
    {
        _userServiceClient = userServiceClient;
        _bandServiceClient = bandServiceClient;
    }

    public async Task<RegisterBandResponse> HandleAsync(RegisterBandRequest request, CancellationToken cancellationToken)
    {
        var account = await _userServiceClient.RegisterAccountAsync(
                                                                    new RegisterAccountRequest(
                                                                        request.Name,
                                                                        request.Email,
                                                                        request.Password,
                                                                       2),
                                                                    cancellationToken);

        if (account is null)
            throw new InvalidOperationException("Failed to create band account.");

        var band = await _bandServiceClient.CreateBandAsync(
                                                            new CreateBandRequest(
                                                                account.Id,
                                                                request.Name,
                                                                request.Genre,
                                                                request.Description,
                                                                request.SpotifyId),
                                                            cancellationToken);

        if (band is null)
            throw new InvalidOperationException("Failed to create band profile.");

        return new RegisterBandResponse(account.Id, band.Id, request.Name, request.Email, request.Genre, request.Description, request.SpotifyId);
    }
}