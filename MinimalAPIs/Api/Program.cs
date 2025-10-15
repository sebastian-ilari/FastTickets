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

if (!builder.Environment.IsEnvironment("Testing"))
{
    var dbFactory = new FastTicketsDBFactory(builder.Configuration);
    await dbFactory.Create();
}

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<ITicketService, TicketService>();

builder.Services.ConfigureHttpJsonOptions(options => {
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    /*
     * Not needed after adding ProblemDetails
    app.UseExceptionHandler(config =>
    {
        config.Run(async context =>
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("An unexpected error occurred");
        });
    });
    */
}

var appGroup = app.MapGroup("/fast-tickets");

appGroup.RegisterShowEndpoints();
appGroup.RegisterTicketEndpoints();

app.Run();

public partial class Program { }
