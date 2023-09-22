using Microsoft.EntityFrameworkCore;
using MyBoards.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyBoardsContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MyBoardsConnectionStrings"))
    );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetService<MyBoardsContext>();

var users = dbContext.Users.ToList();
if (!users.Any())
{
    var user1 = new User()
    {
        FullName = "Antek Nowak",
        Email = "anteknowak@test.tv",
        Address = new Address()
        {
            Country = "Polska",
            City = "Warszawa",
            PostalCode = "00-000"
        },
    };
    var user2 = new User()
    {
        FullName = "Kuba Zawi",
        Email = "kubazawi@test.tv",
        Address = new Address()
        {
            Country = "Rosja",
            City = "Moskwa",
            PostalCode = "11-111"
        }
    };

    dbContext.AddRange(user1, user2);

    dbContext.SaveChanges();
}

var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any())
{
    dbContext.Database.Migrate();
}

app.Run();
