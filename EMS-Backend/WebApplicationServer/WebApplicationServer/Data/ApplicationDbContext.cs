using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplicationServer.Models;
namespace WebApplicationServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<Models.Person>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //public DbSet<Admin> admins { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<BookedEvent> BookedEvents { get; set; }

        public DbSet<Review> Reviews { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                entity.Property(t => t.LoginProvider)
                      .HasMaxLength(450); // Adjust the length to ensure the total key length is within limits

                entity.Property(t => t.Name)
                      .HasMaxLength(450); // Adjust the length to ensure the total key length is within limits
            });



            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                // Adjust the length to ensure the total key length is within limits
                entity.Property(ur => ur.UserId)
                      .HasMaxLength(450);

                entity.Property(ur => ur.RoleId)
                      .HasMaxLength(450);
            });



            builder.Entity<IdentityRole>(entity =>
            {
                entity.Property(r => r.Id)
                      .HasMaxLength(450); // Ensure same length as RoleId in UserRole
            });

            // Configuring IdentityUser entity
            builder.Entity<IdentityUser>(entity =>
            {
                entity.Property(u => u.Id)
                      .HasMaxLength(450); // Ensure length within limit
            });




            builder.Entity<BookedEvent>()
               .HasOne(be => be.User)
               .WithMany()
               .HasForeignKey(be => be.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }

        }
    }
