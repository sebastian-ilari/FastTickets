using API.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.DBFactories;
using Services;

var builder = WebApplication.CreateBuilder(args);

var dbName = builder.Configuration["Database:Application"] 
    ?? throw new ApplicationException("Application database name not set in appsettings.json");

builder.Services.AddDbContext<FastTicketsDB>(options =>
{
    options.UseSqlite($"Data Source={dbName};Mode=Memory;Cache=Shared");
});

var dbFactory = new FastTicketsDBFactory(builder.Configuration);
await dbFactory.Create();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<ITicketService, TicketService>();

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});

var app = builder.Build();

var appGroup = app.MapGroup("/fast-tickets");

appGroup.RegisterShowEndpoints();
appGroup.RegisterTicketEndpoints();

app.Run();

public partial class Program { }
