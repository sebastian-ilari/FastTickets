global using FastEndpoints;
global using FluentValidation;

using API.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.DBFactories;

var builder = WebApplication.CreateBuilder();

var dbName = builder.Configuration["Database:Application"]
    ?? throw new ApplicationException("Application database name not set in appsettings.json");

builder.Services.AddDbContext<FastTicketsDB>(options =>
{
    options.UseSqlite($"Data Source={dbName};Mode=Memory;Cache=Shared");
});

var dbFactory = new FastTicketsDBFactory(builder.Configuration);
await dbFactory.Create();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddFastEndpoints();

builder.Services.AddScoped<ITicketService, TicketService>();

var app = builder.Build();
app.UseDefaultExceptionHandler()
   .UseFastEndpoints(c =>
   {
       c.Endpoints.RoutePrefix = "fast-tickets";
   });
app.Run();

public partial class Program { }
