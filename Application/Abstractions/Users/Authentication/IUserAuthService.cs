using Application.Users.Authentication.Login;
using Application.Users.Authentication.Register;

namespace Application.Abstractions.Users.Authentication;

public interface IUserAuthService
{
    Task<RegisterResult> RegisterLocalUserAsync(RegisterCredentials credentials, CancellationToken ct = default);
    Task<LoginResult> LoginLocalUserAsync(LoginCredentials credentials, CancellationToken ct = default);
    Task LogoutUserAsync(CancellationToken ct = default);
}