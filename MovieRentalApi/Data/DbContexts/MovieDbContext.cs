using Microsoft.EntityFrameworkCore;
using MovieRentalApi.Data.Entities;

namespace MovieRentalApi.Data.DbContexts
{
    public class MovieDbContext : DbContext
    {
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<RentalEntity> Rentals { get; set; }

        public MovieDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
