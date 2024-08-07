using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data
{
    public class DataContext: IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { }

        public DbSet<Url> Urls { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "e8f55c06-ff6e-4d1a-94a6-0dea9d9d8e5a",
                Name = "Ordinary",
                NormalizedName = "Ordinary".ToUpper(),
                ConcurrencyStamp = "e8f55c06-ff6e-4d1a-94a6-0dea9d9d8e5a"
            },
            new IdentityRole()
            {
                Id = "8192ed0c-24b7-4a19-b247-dc12e5d4adb7",
                Name = "Admin",
                NormalizedName = "Admin".ToUpper(),
                ConcurrencyStamp = "8192ed0c-24b7-4a19-b247-dc12e5d4adb7"
            });
        }
    }
}
