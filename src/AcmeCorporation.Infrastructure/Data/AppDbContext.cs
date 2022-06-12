using AcmeCorporation.Core.Entities;
using AcmeCorporation.Infrastructure.Data.Configurations;
using AcmeCorporation.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AcmeCorporation.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.Seed();
        }
    }
}