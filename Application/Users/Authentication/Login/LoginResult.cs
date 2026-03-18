namespace Application.Users.Authentication.Login;

public sealed record LoginResult
{
    private LoginResult(LoginStatus status, string? errorMessage = null)
    {
        Status = status;
        ErrorMessage = errorMessage;
    }

    public LoginStatus Status { get; }

    public bool Succeeded => Status == LoginStatus.Succeeded;

    public string? ErrorMessage { get; }

    public static LoginResult Success() =>
        new(LoginStatus.Succeeded);

    public static LoginResult InvalidCredentials(string? errorMessage = null) =>
        new(LoginStatus.InvalidCredentials, errorMessage ?? "Invalid credentials.");

    public static LoginResult RequiresTwoFactor(string? errorMessage = null) =>
        new(LoginStatus.RequiresTwoFactor, errorMessage ?? "Two-factor authentication is required.");

    public static LoginResult NotAllowed(string? errorMessage = null) =>
        new(LoginStatus.NotAllowed, errorMessage ?? "User is not allowed to sign in.");

    public static LoginResult LockedOut(string? errorMessage = null) =>
        new(LoginStatus.LockedOut, errorMessage ?? "The account is locked out.");
}