using Microsoft.Extensions.Configuration;
using Persistence.Data;

namespace Persistence.DBFactories;

public class FastTicketsDBFactoryTest : FastTicketsDBFactoryBase
{
    public FastTicketsDBFactoryTest(IConfiguration configuration)
    {
        var dbName = configuration["Database:Tests"]
            ?? throw new ApplicationException("Test database name not set in appsettings.json");

        _dbName = dbName;
        _seedData = new SeedTestData();
    }
}
