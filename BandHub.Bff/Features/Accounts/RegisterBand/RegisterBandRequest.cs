namespace BandHub.Bff.Features.Accounts.RegisterBand;

public sealed record RegisterBandRequest(string Name, string Email, string Password, string Genre, string Description, string? SpotifyId);
