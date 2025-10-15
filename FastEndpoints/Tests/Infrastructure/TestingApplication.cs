using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Tests.Common;

namespace Tests.Infrastructure;

public class TestingApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var dbName = ConfigurationHelper.GetConfiguration()["Database:Tests"]
            ?? throw new ApplicationException("Test database name not set in appsettings.json");

        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.AddDbContext<FastTicketsDB>(opt =>
                opt.UseSqlite($"Data Source={dbName};Mode=Memory;Cache=Shared"));
        });

        return base.CreateHost(builder);
    }
}
