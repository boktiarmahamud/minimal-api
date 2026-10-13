using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;
using MinimalApiCURD.Data;
using MinimalApiCURD.DTOs;
using MinimalApiCURD.Models;
using System.Net;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("db")));

// jwt settings
var jwtqSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtqSettings["SecretKey"]?? throw new InvalidOperationException("SecretKey is not configured in appsettings.json");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtqSettings["Issuer"],
        ValidAudience = jwtqSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero // Optional: Set clock skew to zero for immediate expiration
    };
});

builder.Services.AddAuthorization();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

// Register endpoint
app.MapPost("/api/register", async (
    AuthDTOs.RegisterRequest request,
    ApplicationDbContext db,
    IPasswordHasher<User> passwordHasher) =>
{
    if (string.IsNullOrWhiteSpace(request.UserName) ||
        string.IsNullOrWhiteSpace(request.Email) ||
        string.IsNullOrEmpty(request.Password))
    {
        return Results.BadRequest("All fields are required.");
    }

    var email = request.Email.Trim();

    if (await db.Users.AnyAsync(u => u.Email == email))
    {
        return Results.Conflict("Email is already registered.");
    }

    var user = new User
    {
        UserName = request.UserName.Trim(),
        Email = email,
        CreatedAt = DateTime.UtcNow
    };

    user.PasswordHash =
        passwordHasher.HashPassword(user, request.Password);

    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created(
        $"/api/users/{user.Id}",
        new { user.Id, user.UserName, user.Email });
});

// login endpoint
app.MapPost("/api/login", async (
    AuthDTOs.LoginRequest request,
    ApplicationDbContext db,
    IPasswordHasher<User> passwordHasher,
    IConfiguration configuration) =>
{
    if (string.IsNullOrWhiteSpace(request.Email) ||
        string.IsNullOrEmpty(request.Password))
    {
        return Results.BadRequest("Invalid login data.");
    }

    var email = request.Email.Trim();

    var user = await db.Users
        .FirstOrDefaultAsync(u => u.Email == email);

    if (user is null)
    {
        return Results.Unauthorized();
    }

    var result = passwordHasher.VerifyHashedPassword(
        user,
        user.PasswordHash,
        request.Password);

    if (result == PasswordVerificationResult.Failed)
    {
        return Results.Unauthorized();
    }

    var token = GenerateJwtToken(user, configuration);

    return Results.Ok(new AuthDTOs.AuthResponse
    {
        Token = token,
        UserName = user.UserName,
        Email = user.Email
    });
});

app.MapGet("/api/leads",[Authorize] async (ApplicationDbContext db) =>
{
    return await db.Leads.ToListAsync();
});

app.MapGet("/api/leads/{id}", [Authorize] async (int id, ApplicationDbContext db) =>
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

app.MapPost("/api/leads", [Authorize] async (Lead lead, ApplicationDbContext db) =>
{
    lead.CreateAt = DateTime.Now;

    db.Leads.Add(lead);

    await db.SaveChangesAsync();

    return Results.Created($"/api/leads/{lead.id}", lead);
});

app.MapPut("/api/leads/{id}", [Authorize] async (int id, Lead updatedLead, ApplicationDbContext db) =>
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

app.MapDelete("/api/leads/{id}", [Authorize] async (int id, ApplicationDbContext db) =>
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

// Hash password using SHA256

static string HashPassword(string password)
{
    using (var sha256 = System.Security.Cryptography.SHA256.Create())
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}

// Verify password by comparing the hash

static bool VerifyPassword(string password, string hashedPassword)
{
    var hashOfInput = HashPassword(password);
    return StringComparer.Ordinal.Compare(hashOfInput, hashedPassword) == 0;
}

// Generate JWT token

static string GenerateJwtToken(User user, IConfiguration configuration)
{
    var jwtSettings = configuration.GetSection("JwtSettings");
    var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("SecretKey is not configured in appsettings.json");
    var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var key = System.Text.Encoding.UTF8.GetBytes(secretKey);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new System.Security.Claims.ClaimsIdentity(new[]
        {
            new System.Security.Claims.Claim("id", user.Id.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email)
        }),
        Expires = DateTime.UtcNow.AddHours(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        Issuer = jwtSettings["Issuer"],
        Audience = jwtSettings["Audience"]
    };
    return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
}
