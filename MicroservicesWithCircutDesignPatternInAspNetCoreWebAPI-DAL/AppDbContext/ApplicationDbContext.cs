using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.AppDbContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define configurations, relationships, constraints here if needed
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired();
        }
    }
}
