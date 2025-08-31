using Microsoft.EntityFrameworkCore;
using TeaLeavesProductionForecastWebAPI.Model;

namespace TeaLeavesProductionForecastWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Prediction> Predictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>(); // Convert enum to string for DB

            base.OnModelCreating(modelBuilder);
        }
    }
}
