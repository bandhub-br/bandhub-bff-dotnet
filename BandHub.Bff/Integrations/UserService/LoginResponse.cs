namespace BandHub.Bff.Integrations.UserService;

public sealed record LoginResponse(Guid AccountId, string Name, string Email, string AccountType);
