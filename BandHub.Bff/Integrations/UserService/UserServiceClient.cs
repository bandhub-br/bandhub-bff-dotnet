using System.Net.Http.Json;

namespace BandHub.Bff.Integrations.UserService;

public class UserServiceClient
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RegisterAccountResponse?> RegisterAccountAsync(RegisterAccountRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("/accounts/register", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<RegisterAccountResponse>(cancellationToken: cancellationToken);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("/accounts/login", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(body, null, response.StatusCode);
        }

        return (await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken))!;
    }
}