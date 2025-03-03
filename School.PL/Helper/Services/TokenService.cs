using Microsoft.IdentityModel.Tokens;
using School.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School.PL.Helper.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(AppUser appUser)
        {
            var authclaims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,appUser.Id.ToString()),
                new Claim(ClaimTypes.Name,appUser.UserName),
                new Claim(ClaimTypes.Email,appUser.Email),
            };
            var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var token = new JwtSecurityToken
                (
                    issuer: _configuration["Jwt:Issuer"],
                    expires: DateTime.Now.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
                    claims: authclaims,
                    audience: _configuration["Jwt:Audience"],
                    signingCredentials: new SigningCredentials(authkey,SecurityAlgorithms.HmacSha256Signature)

                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
