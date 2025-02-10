using Microsoft.AspNetCore.Identity;
using School.DAL.Models;
using School.PL.Models.UserView;

namespace School.PL.Services
{
    public interface IUserServices
    {
        Task<IEnumerable<UserViewModel>> GetAllUser(string username);
        Task<AppUser> GetUserByIdAsync(Guid id);
        Task<IdentityResult> DeleteUserAsync(Guid id);
        Task<IdentityResult> UpdateUserAsync(Guid id, UserUpdateViewModel appUserUpdate);
        Task<UserUpdateViewModel> GetUserUpdateViewModelAsync(Guid id);
        Task<bool> AssignUserToClassAsync(Guid userId, Guid classId);
    }
}
