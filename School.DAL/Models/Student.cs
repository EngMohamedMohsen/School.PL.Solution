using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace School.DAL.Models
{
    public class Student : IdentityUser
    {

        [Required(ErrorMessage = "Code Is Required")]
        public String S_Code { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        public string S_Name { get; set; }

        [Required(ErrorMessage = "Address Is Required")]
        public string S_Address { get; set; }

        [Phone]
        public string S_PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date Is Required")]
        [DisplayName("Date Of Creation")]
        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        // Many TO Many Relation
        public virtual ICollection<Classes> Classes { get; set; }

    }
}
