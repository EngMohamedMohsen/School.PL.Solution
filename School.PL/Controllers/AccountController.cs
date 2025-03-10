﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using School.DAL.Contexts;
using School.DAL.Models;
using School.PL.Helper.Services;
using School.PL.Models.AccountView;
using System.Security.Claims;
using System.Text.Json;

namespace School.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SchoolDbContext _schoolDbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IRedisService _redisService;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,
                                 ITokenService tokenService,
                                 SchoolDbContext schoolDbContext,
                                 IMemoryCache memoryCache,
                                 IRedisService redisService,HttpClient httpClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _schoolDbContext = schoolDbContext;
            _memoryCache = memoryCache;
            _redisService = redisService;
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
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {
               // Check Email Valid
               var user = await _userManager.FindByEmailAsync(model.Email);
               if (user != null)
               {
                   // Check Password Valid
                   var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
                   if (Flag)
                   {
                       var Result = await _signInManager.PasswordSignInAsync
                           (user, model.Password, model.RememberMe, false
                           ); // Generate Token for this user sign in 
                       if (Result.Succeeded)
                       {
                            var cacheData = user;
                            var cacheKey = $"User_{user.Id}";
                            var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
                            _memoryCache.Set(cacheKey, user, expirationTime);
                            return RedirectToAction("Index", "Home");
                       }
                   }
               }

               ModelState.AddModelError("", "Login Failed");
            }
            return View(model);
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

        //[HttpPost]
        //public async Task<IActionResult> SignIn(SignInViewModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user is null) return Unauthorized("Invalid email or password");
        //    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        //    if (!result.Succeeded) return Unauthorized("Invalid email or password");
        //    var token = _tokenService.CreateToken(user);

        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    return RedirectToAction("Index", "Home");
        //}

        [HttpGet]
        [Authorize]
        public IActionResult TestAuth()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            Console.WriteLine("Received Token: " + token);
            return Ok(token);
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

        public IActionResult GetDataFromCache()
        {
            return View();
        }

        //public async Task<IActionResult> MemoryCache()
        //{
        //    var cacheData = _memoryCache.Get<IEnumerable<AppUser>>("User");
        //    if (cacheData != null)
        //    {
        //        return Json(cacheData);
        //    }

        //    var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
        //    cacheData = await _schoolDbContext.Users.ToListAsync();
        //    _memoryCache.Set("User", cacheData, expirationTime);
        //    return Json(cacheData);
        //}

        [HttpGet]
        [Authorize]
        public IActionResult GetSignInCacheData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var cacheKey = $"User_{userId}";
            if (_memoryCache.TryGetValue(cacheKey, out AppUser cacheData))
            {
                return Ok(cacheData);
            }

            return NotFound("No cached data found for this user.");
        }
        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> GetDataFromCache()
        //{
        //    // الحصول على معرف المستخدم الحالي
        //    var userId = User.FindFirstValue(ClaimTypes.PrimarySid);

        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return Unauthorized(); 
        //    }

        //    var cacheKey = $"User_{userId}";
        //    var cacheData = _memoryCache.Get<AppUser>(cacheKey);

        //    if (cacheData != null)
        //    {
        //        return Ok(cacheData);
        //    }

        //    cacheData = await _schoolDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

        //    if (cacheData == null)
        //    {
        //        return NotFound(); 
        //    }

        //    var expirationTime = DateTimeOffset.Now.AddMinutes(5.0);
        //    _memoryCache.Set(cacheKey, cacheData, expirationTime);

        //    return Ok(cacheData);
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetDataFromRedis()
        {
            var userId = User.FindFirstValue(ClaimTypes.PrimarySid);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // جلب بيانات المستخدم من Redis
            var cacheKey = $"User_{userId}";
            var cachedUserData = await _redisService.GetValueAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedUserData))
            {
                var user = JsonSerializer.Deserialize<AppUser>(cachedUserData);
                return Ok(user);
            }

            // استرجاع المستخدم من قاعدة البيانات إذا لم يكن موجودًا في Redis
            var userFromDb = await _schoolDbContext.Users.FindAsync(userId);
            if (userFromDb == null)
            {
                return NotFound();
            }

            // تخزينه مجددًا في Redis
            var userData = JsonSerializer.Serialize(userFromDb);
            await _redisService.SetValueAsync(cacheKey,
                                             userData,
                                             TimeSpan.FromMinutes(5));

            return Ok(userFromDb);
        }
    }
}
