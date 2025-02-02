using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class GenericRepository<T> : IGenericRepository<T> where T : IdentityUser
    {
        private readonly SchoolDbContext _SchoolDbContext;

        public GenericRepository(SchoolDbContext schoolDbContext)
        {
            this._SchoolDbContext = schoolDbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T)==typeof(Student))
            {
                return (IEnumerable<T>)await _SchoolDbContext.Students.Include(S=>S.Classes).ToListAsync();
            }
            return await _SchoolDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _SchoolDbContext.Set<T>().FindAsync(id);
        }
        public async Task AddAsync(T Entity)
        {
           await _SchoolDbContext.AddAsync(Entity);
        }

        public void Update(T Entity)
        {
            _SchoolDbContext.Update(Entity);
        }

        public void Delete(T Entity)
        {
            _SchoolDbContext.Remove(Entity);
        }


    }
}
