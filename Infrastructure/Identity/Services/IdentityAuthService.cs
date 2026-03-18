namespace Infrastructure.Identity.Services;

public interface IAuthService
{
    Task RegisterUserAsync(CancellationToken ct = default);
    Task LoginUserAsync(CancellationToken ct = default);
    Task LogoutUserAsync(CancellationToken ct = default);
}


public sealed class IdentityAuthService
{

}
