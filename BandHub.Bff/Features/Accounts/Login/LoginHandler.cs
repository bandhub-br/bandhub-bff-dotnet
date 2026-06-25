using BandHub.Bff.Integrations.AuthService;

namespace BandHub.Bff.Features.Accounts.Login;

public class LoginHandler
{
    private readonly AuthServiceClient _authServiceClient;

    public LoginHandler(AuthServiceClient authServiceClient)
    {
        _authServiceClient = authServiceClient;
    }

    public async Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var loginRequest = new Integrations.AuthService.LoginRequest(request.Email, request.Password);
        var result = await _authServiceClient.LoginAsync(loginRequest, cancellationToken);

        return new LoginResponse(
            result.AccountId,
            result.Name,
            result.Email,
            result.AccountType,
            result.AcessToken,
            result.AcessTokenExpiraEm,
            result.RefreshToken,
            result.RefreshTokenExpiraEm);
    }
}
