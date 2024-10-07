using Latihan.Models;
using Microsoft.EntityFrameworkCore;

namespace Latihan.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Profilling> Profilling { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AccountRole> AccountRoles { get; set; }

    }
}
