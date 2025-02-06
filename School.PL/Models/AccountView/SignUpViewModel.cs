using System.ComponentModel.DataAnnotations;

namespace School.PL.Models.AccountView
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "first name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email ")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match password ")]
        public string ConfirmPassword { get; set; }


        [Required(ErrorMessage = "Is Active is required")]
        public bool ISAgree { get; set; }
    }
}
