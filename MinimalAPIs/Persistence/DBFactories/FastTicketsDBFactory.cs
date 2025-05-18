using Microsoft.Extensions.Configuration;
using Persistence.Data;

namespace Persistence.DBFactories;

public class FastTicketsDBFactory : FastTicketsDBFactoryBase
{
    public FastTicketsDBFactory(IConfiguration configuration)
    {
        var dbName = configuration["Database:Application"]
            ?? throw new ApplicationException("Application database name not set in appsettings.json");

        _dbName = dbName;
        _seedData = new SeedData();
    }
}
