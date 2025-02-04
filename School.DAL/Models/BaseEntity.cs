using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.DAL.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
