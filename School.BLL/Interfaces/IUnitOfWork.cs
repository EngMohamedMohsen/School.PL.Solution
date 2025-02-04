using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        public IClassesRepository ClassesRepository { get;}
        Task<int> SaveDataAsync();
    }
}
