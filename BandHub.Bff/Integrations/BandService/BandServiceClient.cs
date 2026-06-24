using System.Net.Http.Json;

namespace BandHub.Bff.Integrations.BandService;

public class BandServiceClient
{
    private readonly HttpClient _httpClient;

    public BandServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CreateBandResponse?> CreateBandAsync(
        CreateBandRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("/bands", request, cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<CreateBandResponse>(cancellationToken: cancellationToken);
    }
}