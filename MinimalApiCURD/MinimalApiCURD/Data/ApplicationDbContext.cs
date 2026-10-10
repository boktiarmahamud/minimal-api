using Microsoft.EntityFrameworkCore;
using MinimalApiCURD.Models;

namespace MinimalApiCURD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

       public DbSet<Lead> Leads { get;set;  }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lead>().HasData(
                new Lead
                {
                    id = 1,
                    FisrtName = "Boktiar",
                    LastName = "Mahamud",
                    Email = "boktiarmahamud@gmail.com",
                    Phone = "+880 1744555821",
                    Comapany = "Tech Solutions",
                    Status = "New",
                    CreateAt = new DateTime(2026, 1, 10)
                }
                );
        }
    }
}
