namespace BandHub.Bff.Integrations.BandService;

public sealed record CreateBandResponse(Guid Id, Guid AccountId, string Name, string Genre, string Description, string? SpotifyId, DateTime CreateAt);
