using Microsoft.EntityFrameworkCore;
using TopTenMoviesAPI.Context.Entities;

namespace TopTenMoviesAPI.Context;

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
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Id).ValueGeneratedOnAdd();
            entity.Property(m => m.Title).IsRequired();
            entity.Property(m => m.Category).IsRequired();
            entity.Property(m => m.Rate).IsRequired();
            entity.Property(m => m.ImagePath).IsRequired(false);
            entity.Property(m => m.CreatedDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
    public void SeedData()
    {
        Movies.AddRange(
            new Movie { Title = "Avatar The Way of Water", Category = "Science Fiction", Rate = 9.1, ImagePath = "https://m.media-amazon.com/images/M/MV5BYjhiNjBlODctY2ZiOC00YjVlLWFlNzAtNTVhNzM1YjI1NzMxXkEyXkFqcGdeQXVyMjQxNTE1MDA@._V1_FMjpg_UX1000_.jpg" },
            new Movie { Title = "Spider-Man Beyond the Spider-Verse", Category = "Animation", Rate = 9.3, ImagePath = "https://m.media-amazon.com/images/M/MV5BNTNlOTU3Y2EtYTg2NC00NmIyLTk1ZDgtNTY0ZjM4NDA1NjgwXkEyXkFqcGdeQXVyMTcyNjczNjU4._V1_.jpg" },
            new Movie { Title = "The Batman Arkham", Category = "Action", Rate = 8.7 , ImagePath = "https://m.media-amazon.com/images/M/MV5BZTExNTU0YTItYjVjMC00NmRmLWI3NzAtYmE2ZmE2ODFkMzM2XkEyXkFqcGdeQXVyNTgyNTA4MjM@._V1_FMjpg_UX1000_.jpg" },
            new Movie { Title = "Guardians of the Galaxy Vol. 3", Category = "Action", Rate = 8.6, ImagePath = "https://image.tmdb.org/t/p/original/r2J02Z2OpNTctfOSN1Ydgii51I3.jpg" },
            new Movie { Title = "Dune Part Two", Category = "Science Fiction", Rate = 8.5, ImagePath = "https://m.media-amazon.com/images/M/MV5BN2QyZGU4ZDctOWMzMy00NTc5LThlOGQtODhmNDI1NmY5YzAwXkEyXkFqcGdeQXVyMDM2NDM2MQ@@._V1_FMjpg_UX1000_.jpg" },
            new Movie { Title = "Mission Impossible Dead Reckoning Two", Category = "Action", Rate = 8.4, ImagePath = "https://images.squarespace-cdn.com/content/v1/5f877806d101051512f22595/022ed488-f318-47a5-a129-d0548956d8e2/photo+1.jpg" },
            new Movie { Title = "The Marvels", Category = "Action", Rate = 8.3, ImagePath = "https://m.media-amazon.com/images/M/MV5BMTE0YWFmOTMtYTU2ZS00ZTIxLWE3OTEtYTNiYzBkZjViZThiXkEyXkFqcGdeQXVyODMzMzQ4OTI@._V1_FMjpg_UX1000_.jpg" },
            new Movie { Title = "Fantastic Beasts The Secrets of Dumbledore", Category = "Fantasy", Rate = 8.2, ImagePath = "https://image.tmdb.org/t/p/original/d5m6TQ4X2M3boOdGOkZn9YQADvO.jpg" },
            new Movie { Title = "Black Panther Wakanda Forever", Category = "Action", Rate = 8.1, ImagePath = "https://m.media-amazon.com/images/M/MV5BNTM4NjIxNmEtYWE5NS00NDczLTkyNWQtYThhNmQyZGQzMjM0XkEyXkFqcGdeQXVyODk4OTc3MTY@._V1_.jpg" },
            new Movie { Title = "Indiana Jones and the Dial of Destiny", Category = "Adventure", Rate = 8.0, ImagePath = "https://m.media-amazon.com/images/M/MV5BY2M0ZGEwMGQtNzMxOC00OTU2LWExZmUtMTA5N2RhMDZlY2JiXkEyXkFqcGdeQXVyODE5NzE3OTE@._V1_FMjpg_UX1000_.jpg" }
        );
        SaveChanges();
    }
}
