using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using School.DAL.Models;
using School.PL.Models.AccountView;

namespace School.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user is null)
                    {
                        user = await _userManager.FindByEmailAsync(model.Email);
                        if (user is null)
                        {
                            user = new AppUser()
                            {
                                UserName = model.UserName,
                                Email = model.Email,
                                ISAgree = model.ISAgree
                            };
                            var result = await _userManager.CreateAsync(user, model.Password);
                            if (result.Succeeded)
                            {
                                return RedirectToAction("SignIn");
                            }
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                        ModelState.AddModelError(string.Empty, "Email Is already exists");
                        return View(model);
                    }
                    ModelState.AddModelError(string.Empty, "UserName Is already exists");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SignIn()
        {

            return View();
        }

        [HttpPost]

        //public async Task<IActionResult> SignIn(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user is not null)
        //        {
        //            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Index", "Home");

        //            }

        //            ModelState.AddModelError("", "Login Failed");
        //            return View(model);
        //        }
        //    }

        //    return View(model);
        //}

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
    }
}
