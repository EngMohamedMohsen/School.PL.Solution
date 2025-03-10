using Microsoft.AspNetCore.Identity;

namespace School.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public bool ISAgree { get; set; }
        public Guid? ClassesId { get; set;}

        public Classes? Classes {get; set;}
    }
}
