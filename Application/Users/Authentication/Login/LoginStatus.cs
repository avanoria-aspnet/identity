namespace Application.Users.Authentication.Login;

public enum LoginStatus
{
    Succeeded = 0,
    InvalidCredentials,
    RequiresTwoFactor,
    NotAllowed,
    LockedOut
}
