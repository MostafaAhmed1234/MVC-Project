using Business.Logic.Layer.Interfaces;
using Data.Access.Layer.Context;
using Data.Access.Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Business.Logic.Layer.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        #region Repository Pattern
        //private readonly dbContext _dbContext;

        //public DepartmentRepository(dbContext dbContext) //Ask CLR for creating object from dbContext
        //{
        //    //_dbContext = new dbContext();
        //    //-----------------------------------
        //    _dbContext = dbContext;
        //}
        ////---------------------------
        //public int Add(Department department)
        //{
        //    _dbContext.Departments.Add(department);
        //    return _dbContext.SaveChanges(); // to return no. of rows affected
        //}

        //public int Delete(Department department)
        //{
        //    _dbContext.Departments.Remove(department);
        //    return _dbContext.SaveChanges();
        //}
        //public Department GetById(int id)
        //{
        //    //var department = _dbContext.Departments.Local.Where(D => D.Id == id).FirstOrDefault();
        //    //if (department is null)
        //    //    department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();
        //    //return department;

        //    //======= Linq operator works like above, search local then search remote [find(id)]========
        //    return _dbContext.Departments.Find(id);
        //}
        //public IEnumerable<Department> GetAll()
        //    => _dbContext.Departments.ToList();

        //public int Update(Department department)
        //{
        //    //in case, tracking object that i will update or delete, to state will change  
        //    _dbContext.Departments.Update(department);
        //    return _dbContext.SaveChanges();
        //}
        #endregion
        public DepartmentRepository(dbContext dbContext) : base(dbContext) //Ask CLR for creating object from dbContext
        {
        }
    }
}
