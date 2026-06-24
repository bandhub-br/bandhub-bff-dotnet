namespace BandHub.Bff.Integrations.UserService;

public sealed record RegisterAccountRequest(string Name, string Email, string Password, int AccountType);
