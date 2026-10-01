using System.Text;
using Data;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service;
using Service.Interfaces;
using Service.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Database

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


// Data Protection

builder.Services.AddDataProtection();


// Identity

builder.Services
    .AddIdentityCore<User>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// JWT Authentication

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });


// Services

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();


// Repositories

builder.Services.AddScoped(
    typeof(IBaseRepository<>),
    typeof(BaseRepository<>));

builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();


// AutoMapper

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile));


// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7172")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// Controllers

builder.Services.AddControllers();


// Swagger / OpenAPI

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();


var app = builder.Build();


// Development

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// Swagger

app.UseSwagger();
app.UseSwaggerUI();


// Middleware

app.UseHttpsRedirection();

app.UseCors("ClientPolicy");

app.UseAuthentication();

app.UseAuthorization();


// Controllers

app.MapControllers();

app.Run();