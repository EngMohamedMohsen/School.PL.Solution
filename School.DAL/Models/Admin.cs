using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DAL.Models
{
    public class Admin
    {
        public int Admin_Id { get; set; }
        public string Admin_Name { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
