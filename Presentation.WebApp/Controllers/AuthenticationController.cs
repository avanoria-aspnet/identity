using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Authentication.Register;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController : Controller
{

    [HttpGet("sign-up")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("sign-up")]
    public IActionResult SignUp(RegisterEmailForm form)
    {
        return View();
    }
}
