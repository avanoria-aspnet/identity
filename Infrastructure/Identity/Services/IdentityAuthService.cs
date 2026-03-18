using Application.Abstractions.Users;
using Application.Users.Authentication.Login;
using Application.Users.Authentication.Register;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Services;


public sealed class IdentityAuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IUserAuthService
{
    public async Task<RegisterResult> RegisterLocalUserAsync(RegisterCredentials credentials, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(credentials);

        var email = credentials.Email?.Trim();

        if (string.IsNullOrWhiteSpace(email))
            return RegisterResult.InvalidInput(["Email is required."]);

        if (string.IsNullOrWhiteSpace(credentials.Password))
            return RegisterResult.InvalidInput(["Password is required."]);

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null)
            return RegisterResult.UserAlreadyExists();

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var createResult = await userManager.CreateAsync(user, credentials.Password);

        if (!createResult.Succeeded)
        {
            if (createResult.Errors.Any(static x => x.Code is nameof(IdentityErrorDescriber.DuplicateEmail) or nameof(IdentityErrorDescriber.DuplicateUserName)))
                return RegisterResult.UserAlreadyExists();

            var errors = createResult.Errors
                .Select(static x => x.Description)
                .Where(static x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToArray();

            return errors.Length > 0
                ? RegisterResult.InvalidInput(errors)
                : RegisterResult.Failed();
        }

        var isConfirmed = await userManager.IsEmailConfirmedAsync(user) || await userManager.IsPhoneNumberConfirmedAsync(user);

        return isConfirmed
            ? RegisterResult.CreatedAndConfirmed(user.Id)
            : RegisterResult.CreatedPendingConfirmation(user.Id);
    }

    public async Task<LoginResult> LoginLocalUserAsync(LoginCredentials credentials, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(credentials);

        var email = credentials.Email?.Trim();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(credentials.Password))
            return LoginResult.InvalidCredentials("Email and password are required.");

        var signInResult = await signInManager.PasswordSignInAsync(email,credentials.Password, credentials.RememberMe, lockoutOnFailure: true);

        if (signInResult.Succeeded)
            return LoginResult.Success();

        if (signInResult.RequiresTwoFactor)
            return LoginResult.RequiresTwoFactor();

        if (signInResult.IsNotAllowed)
            return LoginResult.NotAllowed();

        if (signInResult.IsLockedOut)
            return LoginResult.LockedOut();

        return LoginResult.InvalidCredentials();
    }

    public Task LogoutUserAsync(CancellationToken ct = default) =>
        signInManager.SignOutAsync();
}
