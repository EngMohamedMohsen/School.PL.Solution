using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.BLL.Interfaces
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAllData();

        T GetById(int id);
        void Add(T Entity);
        void Update(T Entity);
        void Delete(T Entity);
    }
}
