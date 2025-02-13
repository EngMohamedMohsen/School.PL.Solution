using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.BLL.Interfaces;
using School.DAL.Models;
using School.PL.Controllers;
using School.PL.Models.UserView;

namespace School.PL.Helper.Services
{
    public class UserServices : IUserServices
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserController> _logger;

        public UserServices(UserManager<AppUser> userManager, ILogger<UserController> logger, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUser(string username)
        {
            var users = Enumerable.Empty<UserViewModel>();
            if (string.IsNullOrEmpty(username))
            {
                users = await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = Guid.Parse(U.Id),
                    UserName = U.UserName,
                    Email = U.Email,
                    ClassName = U.Classes.Name
                }).ToListAsync();
            }
            else
            {
                users = await _userManager.Users.Where(U => U.Email.ToLower()
                                  .Contains(username.ToLower()))
                                  .Select(U => new UserViewModel()
                                  {
                                      Id = Guid.Parse(U.Id),
                                      UserName = U.UserName,
                                      Email = U.Email,
                                      ClassName = U.Classes.Name
                                  }).ToListAsync();

            }
            return users;
        }

        public async Task<AppUser> GetUserByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityResult> UpdateUserAsync(Guid id, UserUpdateViewModel appUserUpdate)
        {

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                _logger.LogWarning("User not found with id: {id}", id);
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            user.UserName = appUserUpdate.UserName;
            user.NormalizedUserName = appUserUpdate.UserName.ToUpper();

            return await _userManager.UpdateAsync(user);

        }

        public async Task<UserUpdateViewModel> GetUserUpdateViewModelAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return new UserUpdateViewModel
            {
                Id = id,
                UserName = user.UserName
            };
        }


        public async Task<bool> AssignUserToClassAsync(Guid userId, Guid classId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var selectedClass = await _unitOfWork.ClassesRepository.GetByIdAsync(classId);

            if (user != null && selectedClass != null)
            {
                user.Classes = selectedClass;
                await _unitOfWork.SaveDataAsync();
                return true;
            }
            return false;
        }

        public async Task<IdentityResult> DeleteUserAsync(Guid id)
        {
            try
            {
                // Try to find the user by ID
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    // If the user is not found, return a failed IdentityResult
                    return IdentityResult.Failed(new IdentityError { Description = "User not found" });
                }

                // Delete the user
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // Log errors if the deletion fails
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError(error.Description);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                _logger.LogError(ex.Message);
                return IdentityResult.Failed(new IdentityError { Description = "An error occurred while deleting the user." });
            }
        }
    }
}
