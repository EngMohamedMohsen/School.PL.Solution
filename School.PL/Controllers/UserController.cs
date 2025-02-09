using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.BLL.Interfaces;
using School.DAL.Contexts;
using School.DAL.Models;
using School.PL.Models.UserView;

namespace School.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<AppUser> userManager, ILogger<UserController> logger,IUnitOfWork unitOfWork )
        {
            _userManager = userManager;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string searchInput)
        {
            var users=Enumerable.Empty<UserViewModel>();
            if (string.IsNullOrEmpty(searchInput))
            {
                users=await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id=Guid.Parse(U.Id),
                    UserName=U.UserName,
                    Email=U.Email,
                    ClassName=U.Classes.Name
                }).ToListAsync();
            }
            else
            {
                users=await _userManager.Users.Where(U => U.Email.ToLower()
                                  .Contains(searchInput.ToLower()))
                                  .Select(U => new UserViewModel()
                                  {
                                      Id = Guid.Parse(U.Id),
                                      UserName = U.UserName,
                                      Email = U.Email,
                                      ClassName = U.Classes.Name
                                  }).ToListAsync();

            }
            return View(users);
        }

        public async Task<IActionResult> Details(Guid? id, string viewname = "Details")
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user is null)
            {
                return NotFound();
            }
            if (viewname == "Update")
            {

                var UserModel = new UserUpdateViewModel
                {
                    Id =Guid.Parse(user.Id),
                    UserName = user.UserName
                };
                return View(viewname, UserModel);
            }
            return View(viewname, user);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            return await Details(id, "Update");

        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UserUpdateViewModel appUserUpdate)
        {
            if (id != appUserUpdate.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id.ToString());
                    if (user is null)
                    {
                        return NotFound();
                    }
                    user.UserName = appUserUpdate.UserName;
                    user.NormalizedUserName = appUserUpdate.UserName.ToUpper();
                    var res = await _userManager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        _logger.LogInformation("user updated successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogInformation("user updated Failed");
                        return View(appUserUpdate);

                    }


                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);

                }

            }

            return View(appUserUpdate);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user is null)
                {
                    return NotFound();
                }
                var res = await _userManager.DeleteAsync(user);
                if (res.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var i in res.Errors)
                {
                    _logger.LogError(i.Description);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }


            return RedirectToAction(nameof(Index));


        }

        [HttpGet]
        public async Task<IActionResult> AssignToClassAsync()
        {
            var classes = await _unitOfWork.ClassesRepository.GetAllAsync();

            // Return a view with classes and an empty AppUser ID (for selecting the user)
            ViewData["Classes"] = new SelectList(classes, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToClass(Guid? userId, Guid? classId)
        {
            try
            {
                if (userId is null || classId is null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid user or class.");
                    return View();
                }

                // Fetch the user by userId
                var user = await _userManager.FindByIdAsync(userId.ToString());
                // Check userId Valid !!
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View();
                }

                // Fetch the class by classId
                var selectedClass = await _unitOfWork.ClassesRepository.GetByIdAsync(classId);

                // Assign user to the class
                user.Classes = selectedClass;

                // Save changes to the database
                await _unitOfWork.SaveDataAsync();

                // Redirect to a success page or any other desired page
                return RedirectToAction(nameof(Success));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }

        // A simple success view after assignment
        public IActionResult Success()
        {
            return View();
        }
    }
}
   
