namespace Application.Users.Authentication.Register;

public enum RegisterStatus
{
    CreatedAndConfirmed = 0,
    CreatedPendingConfirmation,
    CreatedWithoutPassword,
    InvalidInput,
    UserAlreadyExists,
    Failed,
    NotFound
}
