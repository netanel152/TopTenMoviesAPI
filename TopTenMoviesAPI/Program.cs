using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Serilog;
using TopTenMoviesAPI.Context;
using TopTenMoviesAPI.Repositories;
using TopTenMoviesAPI.Repositories.Interfaces;
using TopTenMoviesAPI.Services;
using TopTenMoviesAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Top Ten Movies API", Version = "v1" });
});

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();

var connectionString = builder.Configuration.GetConnectionString("MoviesDatabase");
builder.Services.AddDbContext<MoviesDBContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseCors("AllowAnyOrigin");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDBContext>();
    dbContext.Database.EnsureCreated();
    if (!dbContext.Movies.Any())
    {
        dbContext.SeedData();
    }
}

string binPath = Path.Combine(Directory.GetCurrentDirectory(), "bin", "Debug", "net8.0", "Images");
if (!Directory.Exists(binPath))
{
    Directory.CreateDirectory(binPath);
}

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(binPath),
    RequestPath = "/Images"
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{

    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandlerFeature != null)
        {
            Log.Error($"An unhandled exception occurred: {exceptionHandlerFeature.Error}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    });
});

app.Run();
