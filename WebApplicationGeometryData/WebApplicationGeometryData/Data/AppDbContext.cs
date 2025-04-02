using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using WebApplicationGeometryData.Models;


namespace WebApplicationGeometryData.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GeometryData> Geometries { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GeometryData>()
                .Property(g => g.Geom)
                .HasColumnType("geometry");
        }
    }
}
