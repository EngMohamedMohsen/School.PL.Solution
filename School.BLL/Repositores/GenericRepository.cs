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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SchoolDbContext _SchoolDbContext;

        public GenericRepository(SchoolDbContext schoolDbContext)
        {
            this._SchoolDbContext = schoolDbContext;
        }

        public IEnumerable<T> GetAllData()
        {
            if (typeof(T)==typeof(Student))
            {
                return (IEnumerable<T>)_SchoolDbContext.Students.Include(S=>S.Classes).ToList();
            }
            return _SchoolDbContext.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _SchoolDbContext.Set<T>().Find(id);
        }
        public void Add(T Entity)
        {
            _SchoolDbContext.Add(Entity);
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
