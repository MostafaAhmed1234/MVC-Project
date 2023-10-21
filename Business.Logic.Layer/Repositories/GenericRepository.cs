using Business.Logic.Layer.Interfaces;
using Data.Access.Layer.Context;
using Data.Access.Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class // Set<T>() =>>> works with Entities [Classes]
    {
        //protected =>>> to make EmployeeRepository take the same object after inheritance
        private protected readonly dbContext _dbContext;

        public GenericRepository(dbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //---------------------------
        public async Task Add(T item)
        {
          await  _dbContext.Set<T>().AddAsync(item);
            //return _dbContext.SaveChanges();
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
            //return _dbContext.SaveChanges();
        }
        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                //we write this becuase we can't write it  [_dbContext.Set<T>().Include(E=>E.Department).ToList()] without
                //Specificatin Design Pattern that can write Dynamic Query
                return (IEnumerable<T>) await _dbContext.Employees.Include(E=>E.Department).ToListAsync();
            }
            else
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
            //return _dbContext.SaveChanges();
        }
    }
}
