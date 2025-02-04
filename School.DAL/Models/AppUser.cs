using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DAL.Models
{
    public enum UserType
    {
        Admin,
        Teacher,
        Student
    };
    public class AppUser : IdentityUser
    {
        public UserType Type {get; set;}

        public Guid? ClassesId { get; set;}

        public Classes? Classes {get; set;}
    }
}
