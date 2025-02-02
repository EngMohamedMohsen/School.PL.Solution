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
    public class Classes : IdentityUser
    {

        [Required(ErrorMessage = "Code Is Required")]
        public String C_Code { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        public string C_Name { get; set; }

        [Required(ErrorMessage = "Date Is Required")]
        [DisplayName("Date Of Creation")]
        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        // Many TO Many Relation
        public virtual ICollection<Student> Students { get; set; }


    }
}
