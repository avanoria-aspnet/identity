using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Authentication.Register;

public class RegisterEmailForm
{
    [Required(ErrorMessage = "Email address is required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid format for email address")]
    [Display(Name = "Email Address", Prompt = "name@example.com")]
    public string Email { get; set; } = null!;
}
