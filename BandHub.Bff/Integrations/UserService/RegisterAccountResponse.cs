namespace BandHub.Bff.Integrations.UserService;

public sealed record RegisterAccountResponse(Guid Id, string Name, string Email, string AccountType, DateTime CreatedAt);
