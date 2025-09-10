using DisasterResourceAllocation.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterResourceAllocation.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Area> Areas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Truck>()
            .Property(t => t.AvailableResources)
            .HasColumnType("jsonb");
            modelBuilder.Entity<Truck>()
            .Property(t => t.TravelTimeToArea)
            .HasColumnType("jsonb");

            modelBuilder.Entity<Area>()
            .Property(t => t.RequiredResources)
            .HasColumnType("jsonb");
    
        }
    }
}