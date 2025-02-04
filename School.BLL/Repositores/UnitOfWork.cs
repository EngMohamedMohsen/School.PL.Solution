using Microsoft.EntityFrameworkCore;
using School.BLL.Interfaces;
using School.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.BLL.Repositores
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ClassesRepository _classesRepository;
        private readonly SchoolDbContext _schoolDbContext;

        public UnitOfWork(SchoolDbContext schoolDbContext)
        { 
            _classesRepository=new ClassesRepository(schoolDbContext);
            _schoolDbContext = schoolDbContext;
        }

        public IClassesRepository ClassesRepository => _classesRepository;

        public async Task<int> SaveDataAsync()
        {
            return await _schoolDbContext.SaveChangesAsync();
        }
    }
}
