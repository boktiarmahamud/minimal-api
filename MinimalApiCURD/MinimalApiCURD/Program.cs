using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MinimalApiCURD.Data;
using MinimalApiCURD.Models;

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

app.MapPost("/api/leads", async (Lead lead, ApplicationDbContext db) =>
{
    lead.CreateAt = DateTime.Now;

    db.Leads.Add(lead);

    await db.SaveChangesAsync();

    return Results.Created($"/api/leads/{lead.id}", lead);
});

app.MapPut("/api/leads/{id}", async (int id, Lead updatedLead, ApplicationDbContext db) =>
{
    var lead = await db.Leads.FindAsync(id);
    if (lead != null)
    {
        lead.FirstName = updatedLead.FirstName;
        lead.LastName = updatedLead.LastName;
        lead.Email = updatedLead.Email;
        lead.Phone = updatedLead.Phone;
        lead.Company = updatedLead.Company;
        lead.Status = updatedLead.Status;
        lead.UpdatedAt = DateTime.Now;
        await db.SaveChangesAsync();
        return Results.Ok(lead);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("/api/leads/{id}", async (int id, ApplicationDbContext db) =>
{
    var lead = await db.Leads.FindAsync(id);
    if (lead != null)
    {
        db.Leads.Remove(lead);
        await db.SaveChangesAsync();
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
