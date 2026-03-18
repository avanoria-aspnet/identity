namespace Application.Users.Authentication.Register;


public sealed record RegisterResult
{
    private RegisterResult(RegisterStatus status, string? userId = null, IReadOnlyList<string>? errors = null)
    {
        Status = status;
        UserId = userId;
        Errors = errors ?? [];
    }

    public RegisterStatus Status { get; }

    public bool Succeeded =>
        Status 
            is RegisterStatus.CreatedAndConfirmed
            or RegisterStatus.CreatedPendingConfirmation
            or RegisterStatus.CreatedWithoutPassword;

    public string? UserId { get; }

    public IReadOnlyList<string> Errors { get; }

    public static RegisterResult CreatedAndConfirmed(string userId) =>
        new(RegisterStatus.CreatedAndConfirmed, userId);

    public static RegisterResult CreatedPendingConfirmation(string userId) =>
        new(RegisterStatus.CreatedPendingConfirmation, userId);

    public static RegisterResult CreatedWithoutPassword(string userId) =>
        new(RegisterStatus.CreatedWithoutPassword, userId);

    public static RegisterResult UserAlreadyExists(string? error = null) =>
        new(RegisterStatus.UserAlreadyExists, errors:
        [
            error ?? "A user with the same email already exists."
        ]);

    public static RegisterResult NotFound(string? error = null) =>
    new(RegisterStatus.NotFound, errors:
    [
        error ?? "A user not found."
    ]);

    public static RegisterResult InvalidInput(IEnumerable<string> errors) =>
        new(RegisterStatus.InvalidInput, errors: [.. errors]);

    public static RegisterResult Failed(string? error = null) =>
        new(RegisterStatus.Failed, errors:
        [
            error ?? "Failed to create user."
        ]);
}