using Application.Abstractions.Users.Authentication;
using Application.Users.Authentication.Login;
using Application.Users.Authentication.Register;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Authentication.Login;
using Presentation.WebApp.Models.Authentication.Register;

namespace Presentation.WebApp.Controllers;

public class AuthenticationController(IUserAuthService authService) : Controller
{
    private const string RegisterEmailSessionKey = "RegisterEmailAddress";


    [HttpGet("sign-up")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(RegisterEmailForm form)
    {
        if (!ModelState.IsValid)
            return View(form);

        var registerResult = await authService.CheckEmailExistsAsync(form.Email);
        if (registerResult.Status == RegisterStatus.UserAlreadyExists)
        {
            ViewData["ErrorMessage"] = "User already exists";
            return View(form);
        }

        HttpContext.Session.SetString(RegisterEmailSessionKey, form.Email);

        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet("set-password")]
    public IActionResult SetPassword()
    {
        var email = HttpContext.Session.GetString(RegisterEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        return View();
    }

    [HttpPost("set-password")]
    public async Task<IActionResult> SetPassword(RegisterPasswordForm form)
    {
        var email = HttpContext.Session.GetString(RegisterEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        if (!ModelState.IsValid)
            return View(form);

        var credentials = new RegisterCredentials(email, form.Password);
        var registerResult = await authService.RegisterLocalUserAsync(credentials);

        if (registerResult.Status == RegisterStatus.UserAlreadyExists)
        {
            ViewData["ErrorMessage"] = "User already exsits";
            return View(form);
        }


        return RedirectToAction(nameof(SignIn));
    }


    [HttpGet("sign-in")]
    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignInForm form)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ErrorMessage"] = "Invalid email or password";
            return View(form);
        }

        var loginCredentials = new LoginCredentials(form.Email, form.Password, false);
        var loginResult = await authService.LoginLocalUserAsync(loginCredentials);
        if (!loginResult.Succeeded)
        {
            if (loginResult.Status == LoginStatus.InvalidCredentials)
                ViewData["ErrorMessage"] = "Invalid email or password";

            if (loginResult.Status == LoginStatus.LockedOut)
                ViewData["ErrorMessage"] = "Your account has been locked";

            return View(form);
        }

        return RedirectToAction("My", "Account");
    }
}
