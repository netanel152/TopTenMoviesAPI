using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TopTenMoviesAPI.Models;

namespace TopTenMoviesAPI.Data
{
    public class MoviesDBContext : DbContext
    {

        public string DbPath { get; }

        public MoviesDBContext(DbContextOptions<MoviesDBContext> options) : base(options)
        {
            var basePath = AppContext.BaseDirectory;
            DbPath = Path.Combine(basePath, "movies.db");
        }

        public DbSet<Movie> Movies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasKey(m => m.MovieId);
                entity.Property(m => m.MovieId).ValueGeneratedOnAdd();
                entity.Property(m => m.Title).IsRequired();
                entity.Property(m => m.Category).IsRequired();
                entity.Property(m => m.Rate).IsRequired();
                entity.Property(m => m.Picture).IsRequired(false);
                entity.Property(m => m.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(m => m.LastUpdatedDate).IsRequired(false);
            });
        }
    }
}
