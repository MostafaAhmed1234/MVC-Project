using Business.Logic.Layer.Interfaces;
using Data.Access.Layer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Repositories
{
    public class uintOfWork : IUintOfWork
    {
        //Automatic properties after implmenting interface "IUintOfWork"
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }
        //-----------------
        private readonly dbContext _DbContext;
        //------------------------

        //constructor to initialize properties
        public uintOfWork(dbContext dbContext) //Ask CLR for creating object from dbContext
        {
            EmployeeRepository = new EmployeeRepository(dbContext);
            DepartmentRepository = new DepartmentRepository(dbContext);
            _DbContext = dbContext;
        }
        //--------------------
        public async Task<int> Complete()
        => await _DbContext.SaveChangesAsync();

        public void Dispose()
        {
            _DbContext.Dispose();
        }
    }
}
