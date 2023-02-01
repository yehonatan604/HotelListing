using HotelListing.Api.Configurations;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Middleware;
using HotelListing.Api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Services

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

// Controllers
builder.Services.AddControllers();

// Swager
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Hotel Listing Api",
        Version = "v1"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using Bearer scheme.
                        Enter 'Bearer' [space] and your token in the text input below.
                        Exapmle: 'Bearer 1234abcdeFg'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "0auth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver")
    );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

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
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Keys:Key"]))

    };
});

// Caching
builder.Services.AddResponseCaching(options =>
{
    options.MaximumBodySize = 1024;
    options.UseCaseSensitivePaths = true;
});

builder.Services.AddControllers().AddOData(options =>
{
    options.Select().Filter().OrderBy();
});

#endregion Services

var app = builder.Build();

#region Middleware

// Swager

app.UseSwagger();
app.UseSwaggerUI();

// Exception Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Https
app.UseHttpsRedirection();

// Cors
app.UseCors("AllowAll");

// Caching
app.UseResponseCaching();

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(10),
        };

    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" };

    await next();
});

// Authenetication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// Controllers
app.MapControllers();

#endregion Middleware

app.Run();
