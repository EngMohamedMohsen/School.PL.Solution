using School.BLL.Interfaces;
using School.DAL.Contexts;
using School.DAL.Models;

namespace School.BLL.Repositores
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(SchoolDbContext context) : base(context) { }// ASk CLR Create Object From AppDbContext

    }
}
