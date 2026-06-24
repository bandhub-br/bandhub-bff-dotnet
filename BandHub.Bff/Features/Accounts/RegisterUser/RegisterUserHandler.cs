using BandHub.Bff.Integrations.UserService;

namespace BandHub.Bff.Features.Accounts.RegisterUser;

public class RegisterUserHandler
{
    private readonly UserServiceClient _userServiceClient;

    public RegisterUserHandler(UserServiceClient userServiceClient)
    {
        _userServiceClient = userServiceClient;
    }

    public async Task<RegisterAccountResponse?> HandleAsync(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var accountRequest = new RegisterAccountRequest(request.Name, request.Email, request.Password, 1);
        return await _userServiceClient.RegisterAccountAsync(accountRequest, cancellationToken);
    }
}
