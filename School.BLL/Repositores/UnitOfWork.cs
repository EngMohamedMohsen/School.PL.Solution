using School.BLL.Interfaces;
using School.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.BLL.Repositores
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly TeacherRepository _teacherRepository;
        private readonly AdminRepository _adminRepository;
        private readonly StudentRepository _studentRepository;
        private readonly ClassesRepository _classesRepository;
        private readonly SchoolDbContext _schoolDbContext;

        public UnitOfWork(SchoolDbContext schoolDbContext)
        { 
            _adminRepository=new AdminRepository(schoolDbContext);
            _studentRepository=new StudentRepository(schoolDbContext);
            _classesRepository=new ClassesRepository(schoolDbContext);
            _teacherRepository=new TeacherRepository(schoolDbContext);
            _schoolDbContext = schoolDbContext;
        }
        public IAdminRepository AdminRepository => _adminRepository;
        public IStudentRepository StudentRepository => _studentRepository;
        public IClassesRepository ClassesRepository => _classesRepository;
        public ITeacherRepository TeacherRepository => _teacherRepository;

        public int SaveData()
        {
            return _schoolDbContext.SaveChanges();
        }
    }
}
