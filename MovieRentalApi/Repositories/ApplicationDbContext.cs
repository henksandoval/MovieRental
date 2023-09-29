using Microsoft.EntityFrameworkCore;
using MovieRentalApi.Entities;

namespace MovieRentalApi.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MovieEntity> Movies { get; set; }
    }
}
