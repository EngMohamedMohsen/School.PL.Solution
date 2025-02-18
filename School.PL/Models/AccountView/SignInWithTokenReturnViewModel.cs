using System.ComponentModel.DataAnnotations;

namespace School.PL.Models.AccountView
{
    public class SignInWithTokenReturnViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
