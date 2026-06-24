namespace BandHub.Bff.Integrations.BandService;

public sealed record CreateBandRequest(Guid AccountId, string Genre, string Name, string Description, string? SpotifyId);
