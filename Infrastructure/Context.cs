
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public Context(DbContextOptions items) : base(items)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
    }
}
