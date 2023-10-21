using Data.Access.Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Access.Layer.Context
{
    public class dbContext : IdentityDbContext<ApplicationUser>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //  => optionsBuilder.UseSqlServer("Server=.;Initial Catalog=MvcProject;Integrated Security=true");

        //--------------------------------
        public dbContext(DbContextOptions<dbContext> options):base(options)
        {
            
        }
        //---------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(E => E.Department)
                .HasForeignKey(E => E.Dept_ID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
            //==============
            base.OnModelCreating(modelBuilder);
        }

        //----------------------------------
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //--------------------------------------------
        //Identity
        // but you don't need to write them
        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }
    }
}
