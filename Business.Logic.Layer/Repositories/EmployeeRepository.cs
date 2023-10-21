using Business.Logic.Layer.Interfaces;
using Data.Access.Layer.Context;
using Data.Access.Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(dbContext dbContext) : base(dbContext) 
        {
        }
        //----------------------
        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Employee> GetEmployeesByName(string name)
        => _dbContext.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower()));
    }
}
