using Microsoft.EntityFrameworkCore;
using GameServer.Database;
using Microsoft.EntityFrameworkCore.Diagnostics; // Не забудь этот using!
var builder = WebApplication.CreateBuilder(args);

// 1. Настройка базы данных PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

// 2. Регистрация нашего хешера
builder.Services.AddScoped<IPasswordHasher, BCryptHasher>();

builder.Services.AddControllers();

var app = builder.Build();

// Автоматическое создание базы при старте (миграции)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); 
}

app.MapControllers();
app.Urls.Add("http://0.0.0.0:80");
app.Run();