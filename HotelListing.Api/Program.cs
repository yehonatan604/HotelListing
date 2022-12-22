using HotelListing.Api.Configurations;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Add_Services_to_the_Container

var connectionString = builder.Configuration.GetConnectionString("HotelListingDbCnnectionString");
builder.Services.AddDbContextFactory<HotelDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

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

#endregion Add_Services_to_the_Container

var app = builder.Build();

#region Configure_HTTP_Request_Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

#endregion Configure_HTTP_Request_Pipeline

app.Run();
