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
            entity.Property(m => m.LastUpdatedDate).IsRequired(false);
        });
    }
    public void SeedData()
    {
        Movies.AddRange(
            new Movie { Title = "Avatar: The Way of Water", Category = "Science Fiction", Rate = 9.1, ImagePath = "Images/avatar_way_of_water.png" },
            new Movie { Title = "Spider-Man: Beyond the Spider-Verse", Category = "Animation", Rate = 8.9, ImagePath = "Images/spider_man_beyond_the_spider_verse.png" },
            new Movie { Title = "The Batman: Arkham", Category = "Action", Rate = 8.7 , ImagePath = "Images/the_batman_arkham.png" },
            new Movie { Title = "Guardians of the Galaxy Vol. 3", Category = "Action", Rate = 8.6, ImagePath = "Images/guardians_of_the_galaxy_vol_3.png" },
            new Movie { Title = "Dune: Part Two", Category = "Science Fiction", Rate = 8.5, ImagePath = "Images/dune_part_two.png" },
            new Movie { Title = "Mission: Impossible - Dead Reckoning Part Two", Category = "Action", Rate = 8.4, ImagePath = "Images/mission_impossible_dead_reckoning_part_two.png" },
            new Movie { Title = "The Marvels", Category = "Action", Rate = 8.3, ImagePath = "Images/the_marvels.png" },
            new Movie { Title = "Fantastic Beasts: The Secrets of Dumbledore", Category = "Fantasy", Rate = 8.2, ImagePath = "Images/fantastic_beasts_the_secrets_of_dumbledore.png" },
            new Movie { Title = "Black Panther: Wakanda Forever", Category = "Action", Rate = 8.1, ImagePath = "Images/black_panther_wakanda_forever.png" },
            new Movie { Title = "Indiana Jones and the Dial of Destiny", Category = "Adventure", Rate = 8.0, ImagePath = "Images/indiana_jones_and_the_dial_of_destiny.png" }
        );
        SaveChanges();
    }
}
