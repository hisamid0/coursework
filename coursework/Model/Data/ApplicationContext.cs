using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Model.Data
{
    class ApplicationContext : DbContext
    {
        public DbSet<Package> Packages { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=courseworkdatabase;Trusted_Connection=True;");
        }
    }
}
