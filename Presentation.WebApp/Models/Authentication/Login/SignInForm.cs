using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication.Login;

public class SignInForm
{
    [Required(ErrorMessage = "Email address is required")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email Address", Prompt = "name@example.com")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;
}