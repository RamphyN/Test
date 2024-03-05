using Microsoft.EntityFrameworkCore;
using test1.Models;

namespace test1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Gente> GENTE { get; set; }
    }
}