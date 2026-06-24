using BandHub.Bff.Integrations.UserService;

namespace BandHub.Bff.Features.Accounts.Login;

public class LoginHandler
{
    private readonly UserServiceClient _userServiceClient;

    public LoginHandler(UserServiceClient userServiceClient)
    {
        _userServiceClient = userServiceClient;
    }

    public async Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var loginRequest = new Integrations.UserService.LoginRequest(request.Email, request.Password);
        return await _userServiceClient.LoginAsync(loginRequest, cancellationToken);
    }
}
