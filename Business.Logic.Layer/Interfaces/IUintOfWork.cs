using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Interfaces
{
    public interface IUintOfWork : IDisposable
    {
        //contain signature of properties for each Interface Repository
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        //-----------------------------
        //signature of method like saveChanges in DBContext
        Task<int> Complete();
    }
}
