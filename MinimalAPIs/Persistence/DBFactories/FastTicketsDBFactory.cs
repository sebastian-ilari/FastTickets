using Persistence.Data;

namespace Persistence.DBFactories;

public class FastTicketsDBFactory : FastTicketsDBFactoryBase
{
    public FastTicketsDBFactory()
    {
        _dbName = DbName.ApplicationDb;
        _seedData = new SeedData();
    }
}
