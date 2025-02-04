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
    public class Classes : BaseEntity
    {

        [Required(ErrorMessage = "Code Is Required")]
        public string C_Code { get; set; }

        [Required(ErrorMessage = "Name Is Required")]
        public string C_Name { get; set; }

        public DateTime HiringDate { get; set; }

        public DateTime DateOfCreation { get; set; } = DateTime.Now;

        // One TO Many Relation
        public virtual ICollection<AppUser> Students { get; set; }
    }
}
