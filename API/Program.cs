using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(2),
                errorNumbersToAdd: null
            );
        }
    );
},ServiceLifetime.Transient);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(["https://localhost:4200", "http://localhost:4200"])
    .WithExposedHeaders("content-disposition"));

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;


if (app.Environment.IsDevelopment())
{
    try
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration");
    }
}
try
{
    var context = services.GetRequiredService<DataContext>();
    await SeedData.SeedAdmin(context);
    await SeedData.SeedCustomers(context);
    await SeedData.SeedTeams(context);
    await SeedData.SeedTicketTypes(context);
    await SeedData.SeedPriorities(context);
    await SeedData.SeedProducts(context);
    await SeedData.SeedCustomerProductLists(context);
    await SeedData.SeedTickets(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    Console.WriteLine("======================================================================");
    logger.LogError(ex, "An error occured during seeding");
}

app.Run();

