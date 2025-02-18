using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using School.DAL.Models;
using School.PL.Helper.Services;
using School.PL.Models.AccountView;

namespace School.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        #region SignUp
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
        #endregion

        #region SignIn
        [HttpGet]
        public IActionResult SignIn()
        {

            return View();
        }

        #region MyRegion
        //[HttpPost]
        //public async Task<IActionResult> SignIn(SignInViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Check Email Valid
        //            var user = await _userManager.FindByEmailAsync(model.Email);
        //            if (user != null)
        //            {
        //                // Check Password Valid
        //                var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
        //                if (Flag)
        //                {
        //                    var Result = await _signInManager.PasswordSignInAsync
        //                        (user, model.Password, model.RememberMe, false
        //                        ); // Generate Token for this user sign in 
        //                    if (Result.Succeeded)
        //                    {
        //                        return RedirectToAction("Index", "Home");
        //                    }
        //                }
        //            }
        //            ModelState.AddModelError("", "Login Failed");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, ex.Message);
        //        }
        //    }
        //    return View(model);
        //} 
        #endregion
        #endregion
        #region MyRegion
        //[HttpPost]
        //public async Task<IActionResult> SignIn(SignInViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            // Check Email Valid
        //            var user = await _userManager.FindByEmailAsync(model.Email);
        //            if (user != null)
        //            {
        //                // Check Password Valid
        //                var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        //                if (passwordValid)
        //                {
        //                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        //                    if (result.Succeeded)
        //                    {
        //                        var token = _tokenService.CreateToken(user);

        //                        var too = new SignInWithTokenReturnViewModel()
        //                        {
        //                            Email = model.Email,
        //                            Password = model.Password,
        //                            Token = token
        //                        };

        //                        // Return the success response with the token information
        //                        return RedirectToAction("Index", "Home");
        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError("", "Login failed, incorrect credentials.");
        //                    }
        //                }
        //                else
        //                {
        //                    ModelState.AddModelError("", "Invalid password.");
        //                }
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "User not found.");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, ex.Message);
        //        }
        //    }
        //    return View(model);
        //} 
        #endregion

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return null;
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return null;
            return Ok(new SignInWithTokenReturnViewModel
            {
                Email = model.Email,
                Password = model.Password,
                Token = _tokenService.CreateToken(user)
            });
        }
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
            //return RedirectToAction("Index", "Home");

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
