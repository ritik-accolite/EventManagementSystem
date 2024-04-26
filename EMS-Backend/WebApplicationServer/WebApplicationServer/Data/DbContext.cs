using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace WebApplicationServer.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }
        //public DbSet<Admin> admins { get; set; }
    }
}
