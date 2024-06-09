using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using TopTenMoviesAPI.Data;
using TopTenMoviesAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Top Ten Movies API", Version = "v1" });
});

builder.Services.AddScoped<IMovieRepository, MovieRepository>();

// Configure SQLite database connection
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

//Enable CORS
app.UseCors("AllowAnyOrigin");
string swaggerJsonPath = "/swagger/v1/swagger.json";

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MoviesDBContext>();
    dbContext.Database.EnsureCreated(); // This ensures that the database is created if it does not exist
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
        {
            ["activated"] = false
        };
        c.SwaggerEndpoint(swaggerJsonPath, "TopTenMovies V1");
    }); 
}
else
{
    swaggerJsonPath = $"/WebApi{swaggerJsonPath}";

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
            // Return an appropriate error response to the client
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    });
});

app.Run();
