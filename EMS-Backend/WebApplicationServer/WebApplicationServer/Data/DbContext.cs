using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Models;
namespace WebApplicationServer.Data
{
    //public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    //{
    //    public DbContext(DbContextOptions<DbContext> options) : base(options)
    //    {
    //    }
    //    //public DbSet<Admin> admins { get; set; }
    //}

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
    }
}
