using Microsoft.EntityFrameworkCore;
using TeaLeavesProductionForecastWebAPI.Model;

namespace TeaLeavesProductionForecastWebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
