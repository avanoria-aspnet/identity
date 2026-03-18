using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication.Register;

public class RegisterPasswordForm
{
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include uppercase, lowercase, number and special character")]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Password must be confirmed")]
    [Compare(nameof(Password), ErrorMessage = "Password must match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm Password")]
    public string ConfirmPassword { get; set; } = null!;
}