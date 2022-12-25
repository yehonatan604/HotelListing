using HotelListing.Api.Configurations;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using Serilog;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Add_Services_to_the_Container

// DbContextFactory
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbCnnectionString");
builder.Services.AddDbContextFactory<HotelDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Identity Core
builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>("HotelListingApi")
    .AddEntityFrameworkStores<HotelDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

// Swager
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin()
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod());
});

// Serilog
builder.Host.UseSerilog((ctx, loggerConfiguration) => loggerConfiguration
                                                          .WriteTo.Console()
                                                          .ReadFrom.Configuration(ctx.Configuration));

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

// Repository Pattern
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IAuthManager, AuthManager>();


// JwtBearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime= true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Keys:Key"]))

    };
});

#endregion Add_Services_to_the_Container

var app = builder.Build();

#region Configure_HTTP_Request_Pipeline

// Swager
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Https
app.UseHttpsRedirection();

// Cors
app.UseCors("AllowAll");

// Authenetication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// Controllers
app.MapControllers();

#endregion Configure_HTTP_Request_Pipeline

app.Run();
