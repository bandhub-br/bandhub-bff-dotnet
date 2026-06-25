using System.Net.Http.Json;

namespace BandHub.Bff.Integrations.AuthService;

public class AuthServiceClient
{
    private readonly HttpClient _httpClient;

    public AuthServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("/auth/login", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(body, null, response.StatusCode);
        }

        return (await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: cancellationToken))!;
    }
}
