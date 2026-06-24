using BandHub.Bff.Integrations.BandService;
using BandHub.Bff.Integrations.UserService;

namespace BandHub.Bff.Features.Accounts.RegisterBand;

public sealed record RegisterBandResponse(Guid AccountId, Guid BandId, string Name, string Email, string Genre, string Description, string? SpotifyId);
