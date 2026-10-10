using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MinimalApiCURD.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("db")));

var app = builder.Build();

app.MapGet("/api/leads", async (ApplicationDbContext db) =>
{
    return await db.Leads.ToListAsync();
});

app.MapGet("/api/leads/{id}", async (int id, ApplicationDbContext db) =>
{
    var lead = await db.Leads.FindAsync(id);
    if(lead != null)
    {
        return Results.Ok(lead);
    }
    else
    {
        return Results.NotFound();
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
