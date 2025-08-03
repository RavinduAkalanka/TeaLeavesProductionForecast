using TeaLeavesProductionForecastWebAPI.Data;
using TeaLeavesProductionForecastWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env
Env.Load();
var dbConn = Environment.GetEnvironmentVariable("DB_CONNECTION");
Console.WriteLine($"DB Connection String: {dbConn}");

// Register DbContext as Singleton
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(dbConn), ServiceLifetime.Singleton);

// Register services
builder.Services.AddScoped<IUserService, UserService>();

// Add controllers and configure JSON enum conversion globally
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger (for API testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Optional: disable HTTPS redirection if not needed
// app.UseHttpsRedirection();

// Map controller routes
app.MapControllers();

app.Run();
