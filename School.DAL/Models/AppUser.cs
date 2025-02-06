using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DAL.Models
{
    public class AppUser : IdentityUser
    {
        public bool ISAgree { get; set; }
        public Guid? ClassesId { get; set;}

        public Classes? Classes {get; set;}
    }
}
