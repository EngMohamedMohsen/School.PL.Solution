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
        public int Add(T Entity)
        {
            _SchoolDbContext.Add(Entity);
            return _SchoolDbContext.SaveChanges();
        }

        public int Update(T Entity)
        {
            _SchoolDbContext.Update(Entity);
            return _SchoolDbContext.SaveChanges();
        }

        public int Delete(T Entity)
        {
            _SchoolDbContext.Remove(Entity);
            return _SchoolDbContext.SaveChanges();
        }


    }
}
