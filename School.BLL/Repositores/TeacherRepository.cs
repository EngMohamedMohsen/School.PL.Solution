using School.BLL.Interfaces;
using School.DAL.Contexts;
using School.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.BLL.Repositores
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(SchoolDbContext schoolDbContext) : base(schoolDbContext)
        {
        }
    }
}
