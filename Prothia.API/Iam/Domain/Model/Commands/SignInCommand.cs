namespace Prothia.API.Iam.Domain.Model.Commands;

public sealed record SignInCommand(string Username, string Password);