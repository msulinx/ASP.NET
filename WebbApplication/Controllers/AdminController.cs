using System.Security.Claims;
using Data.Entities;
using Domain.Extensions;
using Domain.FormData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebbApplication.Models;
using IAuthenticationService = Business.Interfaces.IAuthenticationService;

namespace WebbApplication.Controllers;

public class AdminController(
    IAuthenticationService authenticationService,
    SignInManager<UserEntity> signInManager,
    UserManager<UserEntity> userManager)
    : Controller
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

    [HttpGet]
    [Route("admin/adminsignin")]
    public IActionResult AdminSignIn()
    {
        return View(new SignInViewModel());
    }
    
    [HttpPost]
    [Route("admin/adminsignin")]
    public async Task<IActionResult> AdminSignIn(SignInViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var formData = model.MapTo<SignInFormData>();
        
        var user = await _userManager.FindByEmailAsync(formData.Email!);
        if (user == null)
        {
            return View(model);
        }

        // Kontrollerar om användaren är en admin
        if (!await _userManager.IsInRoleAsync(user, "Admin"))
        {
            ModelState.AddModelError(string.Empty, "Only Admin users has access to this page");
            return View(model);
        }

        // Loggar in
        var result = await _authenticationService.SignInAsync(formData);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        // Claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.GivenName, user.FirstName ?? ""),
            new Claim(ClaimTypes.Surname, user.LastName ?? ""),
            new Claim("UserImage", user.UserImage ?? "/images/avatar.svg")
        };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            IdentityConstants.ApplicationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = formData.RememberMe }
        );

        return RedirectToAction("Index", "Dashboard");
    }
}