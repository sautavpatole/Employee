using EmployePortal.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployePortal.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
