using Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.DBFactories;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FastTicketsDB>(options =>
{
    options.UseSqlite($"Data Source={DbName.ApplicationDb};Mode=Memory;Cache=Shared");
});

var dbFactory = new FastTicketsDBFactory();
await dbFactory.Create();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<ITicketService, TicketService>();

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});

var app = builder.Build();

FastTicketsEndpoints.RegisterFastTicketsEndpoints(app);

app.Run();

public partial class Program { }
