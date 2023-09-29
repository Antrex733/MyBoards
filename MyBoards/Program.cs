using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MyBoards.Dto;
using MyBoards.Entities;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
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

app.MapGet("data", async (MyBoardsContext db) =>
{
    /*var statesCount = db.WorkItems
    .GroupBy(x => x.StateId)
    .Select(g => new { stateId = g.Key, count = g.Count() })
    .ToListAsync();

    var epicOnHold = await db.Epics
        .Where(d => d.StateId == 4)
        .OrderBy(p => p.Priority)
        .ToListAsync();

    var biggestCreator = await db.Comments
    .GroupBy(u => u.UserId)
    .Select(b => new {b.Key, count = b.Count() })
    .ToListAsync();

    var userWho = biggestCreator.First(f => f.count == biggestCreator.Max(c => c.count));

    var Who = db.Users.First(u => u.Id == userWho.Key);
    */

    var states = db.WorkItemStates
    .FromSqlRaw(@"
    SELECT wis.Id, wis.Value
    FROM WorkItemStates wis
    JOIN WorkItems wi on wi.StateId = wis.Id
    GROUP BY wis.Id, wis.Value
    HAVING COUNT(*) > 85
    ");
     /*
    var user = await db.Users
    .Include(c => c.Comments).ThenInclude(w => w.WorkItem)
    .Include(a => a.Address)
    .FirstAsync(u => u.Id == Guid.Parse("68366DBE-0809-490F-CC1D-08DA10AB0E61"));
    var userComments = user.Comments;
    */
    return states;
});

app.MapGet("dataTop", async (MyBoardsContext db) =>
{
    var topAuthors = db.ViewTopAuthors.ToList();

    return topAuthors;
});

app.MapGet("pagination", async (MyBoardsContext db) =>
{
    // user input
    var filter = "a";
    string sortBy = "FullName";
    bool sortByDescending = false;
    int pageNumber = 1;
    int pageSize = 10;
    //

    var query = db.Users
        .Where(u => filter == null ||
            (u.Email.ToLower().Contains(filter.ToLower())
            || u.FullName.ToLower().Contains(filter.ToLower())));

    var totalCount = query.Count();

    if (sortBy != null)
    {
        var columsSelector = new Dictionary<string, Expression<Func<User, object>>>()
        {
            {nameof(User.Email), user => user.Email },
            {nameof(User.FullName), user => user.FullName }
        };

        var sortByExpression = columsSelector[sortBy];

        query = sortByDescending 
        ? query.OrderByDescending(sortByExpression) 
        : query.OrderBy(sortByExpression);
    }
    var result = query.Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();

    var pagedResult = new PagedResult<User>(result, totalCount, pageSize, pageNumber);

    return pagedResult;
});

app.MapPost("update", async (MyBoardsContext db) =>
{
    var epic = await db.Epics.FirstAsync(ep => ep.Id == 1);

    var rejectedState = await db.WorkItemStates.FirstAsync(u => u.Value == "Rejected");

    epic.State = rejectedState;

    await db.SaveChangesAsync();

    return epic;
});

app.MapPost("create", async (MyBoardsContext db) =>
{
    var address = new Address()
    {
        City = "Krakow",
        Country = "Poland",
        Street = "Dluga"
    };

    var user = new User()
    {
        Email = "user@test.com",
        FullName = "Test User",
        Address = address
    };

    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();

    
});

app.MapDelete("delete", async (MyBoardsContext db) =>
{
    var user = await db.Users
    .Include(a => a.Comments)
    .FirstAsync(u => u.Id == Guid.Parse("DC231ACF-AD3C-445D-CC08-08DA10AB0E61"));

    db.Users.Remove(user);

    await db.SaveChangesAsync();
});

app.Run();
