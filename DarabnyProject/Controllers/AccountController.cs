using DarabnyProject.Models;
using System.Security.Claims;
using DarabnyProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser>   _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController>      _logger;

        public AccountController(
            UserManager<ApplicationUser>   userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController>     logger)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _logger        = logger;
        }

        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View("LoginPage", new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("LoginPage", vm);

            var result = await _signInManager.PasswordSignInAsync(
                vm.Email, vm.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} signed in successfully.", vm.Email);
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Email} account locked out.", vm.Email);
                ModelState.AddModelError(string.Empty, "Account is locked out.");
                return View("LoginPage", vm);
            }

            if (result.IsNotAllowed)
            {
                _logger.LogWarning("User {Email} is not allowed to sign in.", vm.Email);
                ModelState.AddModelError(string.Empty, "Sign-in not allowed. Please confirm your account or contact support.");
                return View("LoginPage", vm);
            }

            if (result.RequiresTwoFactor)
            {
                _logger.LogInformation("User {Email} requires two-factor authentication.", vm.Email);
                ModelState.AddModelError(string.Empty, "Two-factor authentication required.");
                return View("LoginPage", vm);
            }

            _logger.LogInformation("Invalid login attempt for {Email}.", vm.Email);
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View("LoginPage", vm);
        }

        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View("RegisterPage", new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("RegisterPage", vm);

            var exists = await _userManager.FindByEmailAsync(vm.Email);
            if (exists != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View("RegisterPage", vm);
            }

            var user = new ApplicationUser
            {
                Name        = vm.Name,
                UserName    = vm.Email,
                Email       = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                Role        = vm.Role,
                SignUpDate  = DateTime.Now,
                IsActive    = true
            };

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View("RegisterPage", vm);
            }

            var isFirstUser = _userManager.Users.Count() == 1;
            var roleName = isFirstUser
                ? "Admin"
                : vm.Role switch
                {
                    Enums.UserRole.Teacher => "Teacher",
                    _                     => "Student"
                };
            await _userManager.AddToRoleAsync(user, roleName);
            await _userManager.AddClaimAsync(user, new Claim("FullName", user.Name));
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
