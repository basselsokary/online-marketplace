using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.Logout;
using Application.Features.Users.Commands.Register;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers.Base;
using Web.Models.Auth;
using Web.Services.Dispatchers;

namespace Web.Controllers;

public class AuthController(IRequestDispatcher dispatcher) : BaseController(dispatcher)
{
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel, string? ReturnUrl = default)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var command = new LoginCommand(viewModel.Email, viewModel.Password, viewModel.RememberMe);
        var result = await Dispatcher.DispatchAsync(command);
        if (result.Succeeded)
        {
            TempData["Message"] = "Login successful!";
            return RedirectToAction("Index", "Home");
        }

        AddModelErrors(result);

        return View(command);
    }

    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var command = new RegisterCommand(
            viewModel.UserName,
            viewModel.Email,
            viewModel.Password,
            viewModel.FullName,
            viewModel.PhoneNumber,
            viewModel.Address,
            viewModel.Age);
        var result = await Dispatcher.DispatchAsync(command);
        if (result.Succeeded)
        {
            TempData["Message"] = "Registration successful!";
            return RedirectToAction(nameof(Login));
        }

        AddModelErrors(result);
        return View(command);
    }

    public async Task<IActionResult> Logout()
    {
        await Dispatcher.DispatchAsync(new LogoutCommand());

        TempData["Message"] = "Logout successful!";
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied()
    {
        return View(nameof(AccessDenied));
    }
}
