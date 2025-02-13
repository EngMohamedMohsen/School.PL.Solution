using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.DAL.Models;
using School.PL.Helper.CustomAttributes;
using School.PL.Models.RoleView;

namespace School.PL.Controllers
{
    [CustomAuthorize("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, ILogger<UserController> logger,UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: RoleController/Index
        public async Task<IActionResult> Index(string searchInput)
        {
            var Role=Enumerable.Empty<RoleViewModel>();
            if (string.IsNullOrEmpty(searchInput))
            {
                Role =await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id=Guid.Parse(R.Id),
                    RoleName=R.Name
                }).ToListAsync();
            }
            else
            {
                Role = await _roleManager.Roles
                    .Where(R => R.Name
                    .ToLower().Contains(searchInput.ToLower()))
                    .Select(R => new RoleViewModel()
                    {
                        Id = Guid.Parse(R.Id),
                        RoleName = R.Name
                    }).ToListAsync();
            }
            return View(Role);
        }

        // GET: RoleController/Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid? id, string viewname = "Details")
        {
            var Role = await _roleManager.FindByIdAsync(id.ToString());
            if (Role is null)
            {
                return NotFound();
            }
            if (viewname == "Update")
            {

                var RoleModel = new RoleViewModel
                {
                    Id=Guid.Parse(Role.Id),
                    RoleName =Role.Name
                };
                return View(viewname, RoleModel);
            }
            return View(viewname, Role); 
        }

        // GET: RoleController/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name = model.RoleName
                };
                await _roleManager.CreateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: RoleController/Update
        [HttpGet]
        public async Task<IActionResult> Update(Guid? id)
        {
            return await Details(id, "Update");

        }

        // POST: RoleController/Update
        [HttpPost]
        public async Task<IActionResult> Update(Guid? id, RoleViewModel roleViewModel)
        {
            if (id != roleViewModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var Role = await _roleManager.FindByIdAsync(id.ToString());
                    if (Role is null)
                    {
                        return NotFound();
                    }
                    Role.Name = roleViewModel.RoleName;
                    Role.NormalizedName = roleViewModel.RoleName.ToUpper();
                    var res = await _roleManager.UpdateAsync(Role);
                    if (res.Succeeded)
                    {
                        _logger.LogInformation("user updated successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogInformation("user updated Failed");
                        return View(roleViewModel);

                    }


                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);

                }

            }

            return View(roleViewModel);
        }

        // GET: RoleController/Delete
        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                var user = await _roleManager.FindByIdAsync(id.ToString());
                if (user is null)
                {
                    return NotFound();
                }
                var res = await _roleManager.DeleteAsync(user);
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

        // GET: RoleController/AddOrRemoveUsers
        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(Guid? roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role is null)
            {
                return NotFound();
            }
            ViewData["RoleID"]= roleId;
            var usersInRole=new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected=false;
                }
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }

        // POST: RoleController/AddOrRemoveUsers
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(Guid? roleId, List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role is null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appuser = await _userManager.FindByIdAsync(user.UserId);
                    if (appuser is not null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appuser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appuser, role.Name);

                        }

                    }

                }
                return RedirectToAction("Update", new { id = roleId });
            }
            return View(users);
        } 
    }
}
