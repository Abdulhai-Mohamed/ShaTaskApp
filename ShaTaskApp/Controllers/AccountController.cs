using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShaTaskApp.Models.Account;

namespace ShaTaskApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(
      UserManager<IdentityUser> userManager,
      SignInManager<IdentityUser> signInManager
      )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                //1-
                IdentityUser user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                };

                //2-
                // Store user data in AspNetUsers database table
                IdentityResult identityResult = await userManager.CreateAsync(user, model.Password);

                if (identityResult.Succeeded)
                {
                    ViewBag.AlertTitle = "Registration successful!";
                    ViewBag.AlertMessage = "You can Login now, please go to login page";
                    return View("AlertView");
                }
                else
                {
                    // If there are any errors, add them to the ModelState object
                    // which will be displayed by the validation summary tag helper
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl,
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if
                (
                    user == null ||
                    !(await userManager.CheckPasswordAsync(user, model.Password))
                )
                {
                    ModelState.AddModelError(string.Empty, "Wrong Email or password");

                    return View(model);
                }
                var s = await userManager.CheckPasswordAsync(user, model.Password);
                Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(
                user.UserName, model.Password, model.RememberMe, false);

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ops Invalid Login Attempt");
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("InvoicesList", "Business");
        }
    }
}