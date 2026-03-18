namespace Application.Users.Authentication.Login;

public sealed record LoginCredentials
(
    string Email,
    string Password,
    bool RememberMe
);
