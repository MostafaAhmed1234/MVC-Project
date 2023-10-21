using Data.Access.Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Interfaces
{
    public interface IEmployeeRepository :IGenericRepository<Employee>
    {
      IQueryable<Employee> GetEmployeesByAddress(string address);
      IQueryable<Employee> GetEmployeesByName(string name);
    }
}
