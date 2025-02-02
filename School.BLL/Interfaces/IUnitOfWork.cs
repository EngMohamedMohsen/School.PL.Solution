using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IAdminRepository  AdminRepository    { get;}

        public IStudentRepository StudentRepository { get;}
        
        public IClassesRepository ClassesRepository { get;}
        
        public ITeacherRepository TeacherRepository { get;}
        
        Task<int> SaveDataAsync();
    }
}
