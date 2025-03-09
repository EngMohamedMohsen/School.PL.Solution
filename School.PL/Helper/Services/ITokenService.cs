using School.DAL.Models;

namespace School.PL.Helper.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
        bool ValidateToken(string authToken);
    }
}
