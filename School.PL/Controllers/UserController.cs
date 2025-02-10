using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.BLL.Interfaces;
using School.DAL.Contexts;
using School.DAL.Models;
using School.PL.Models.UserView;
using School.PL.Services;

namespace School.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IClassesServices _classesServices;
        private ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger,IUserServices userServices,IClassesServices classesServices )
        {
            _logger = logger;
            _userServices = userServices;
            _classesServices = classesServices;
        }
        public async Task<IActionResult> Index(string searchInput)
        {
            #region Index
            //var users=Enumerable.Empty<UserViewModel>();
            //if (string.IsNullOrEmpty(searchInput))
            //{
            //    users=await _userManager.Users.Select(U => new UserViewModel()
            //    {
            //        Id=Guid.Parse(U.Id),
            //        UserName=U.UserName,
            //        Email=U.Email,
            //        ClassName=U.Classes.Name
            //    }).ToListAsync();
            //}
            //else
            //{
            //    users=await _userManager.Users.Where(U => U.Email.ToLower()
            //                      .Contains(searchInput.ToLower()))
            //                      .Select(U => new UserViewModel()
            //                      {
            //                          Id = Guid.Parse(U.Id),
            //                          UserName = U.UserName,
            //                          Email = U.Email,
            //                          ClassName = U.Classes.Name
            //                      }).ToListAsync();

            //} 
            #endregion
            var user=await _userServices.GetAllUser(searchInput);
            return View(user);
        }

        public async Task<IActionResult> Details(Guid? id, string viewname = "Details")
        {
            if (id == null)
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _userServices.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            if (viewname == "Update")
            {
                var userUpdateModel = await _userServices.GetUserUpdateViewModelAsync(id.Value);
                if (userUpdateModel == null)
                {
                    return NotFound();
                }
                return View(viewname, userUpdateModel);
            }

            return View(viewname, user);
            #region MyRegion
            //var user = await _userManager.FindByIdAsync(id.ToString());
            //if (user is null)
            //{
            //    return NotFound();
            //}
            //if (viewname == "Update")
            //{

            //    var UserModel = new UserUpdateViewModel
            //    {
            //        Id =Guid.Parse(user.Id),
            //        UserName = user.UserName
            //    };
            //    return View(viewname, UserModel);
            //}
            //return View(viewname, user); 
            #endregion

        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            return await Details(id, "Update");

        }
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, UserUpdateViewModel appUserUpdate)
        {
            #region MyRegion
            //if (id != appUserUpdate.Id)
            //{
            //    return NotFound();
            //}
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var user = await _userManager.FindByIdAsync(id.ToString());
            //        if (user is null)
            //        {
            //            return NotFound();
            //        }
            //        user.UserName = appUserUpdate.UserName;
            //        user.NormalizedUserName = appUserUpdate.UserName.ToUpper();
            //        var res = await _userManager.UpdateAsync(user);
            //        if (res.Succeeded)
            //        {
            //            _logger.LogInformation("user updated successfully");
            //            return RedirectToAction(nameof(Index));
            //        }
            //        else
            //        {
            //            _logger.LogInformation("user updated Failed");
            //            return View(appUserUpdate);

            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogInformation(ex.Message);

            //    }

            //}

            //return View(appUserUpdate); 
            #endregion

            if (id != appUserUpdate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userServices.UpdateUserAsync(id.Value, appUserUpdate);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User updated successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogWarning("User update failed");
                        ModelState.AddModelError(string.Empty, "Update failed");
                        return View(appUserUpdate);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating user");
                    return StatusCode(500, "Internal server error");
                }
            }
            return View(appUserUpdate);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _userServices.DeleteUserAsync(id);
            return RedirectToAction(nameof(Index));
            #region MyRegion
            //try
            //{
            //    var user = await _userManager.FindByIdAsync(id.ToString());
            //    if (user is null)
            //    {
            //        return NotFound();
            //    }
            //    var res = await _userManager.DeleteAsync(user);
            //    if (res.Succeeded)
            //    {
            //        return RedirectToAction(nameof(Index));
            //    }
            //    foreach (var i in res.Errors)
            //    {
            //        _logger.LogError(i.Description);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //}
            //return RedirectToAction(nameof(Index)); 
            #endregion
        }

        [HttpGet]
        public async Task<IActionResult> AssignToClassAsync()
        {
            var classes = await _classesServices.GetAllClassAsync();
            ViewData["Classes"] = new SelectList(classes, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignToClass(Guid? userId, Guid? classId)
        {
            if (userId is null || classId is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid user or class.");
                return View();
            }

            try
            {
                var result = await _userServices.AssignUserToClassAsync(userId.Value, classId.Value);
                if (result)
                {
                    return RedirectToAction(nameof(Success));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to assign user to class.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        // A simple success view after assignment
        public IActionResult Success()
        {
            return View();
        }
    }
}
   
