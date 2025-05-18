using Persistence.Data;

namespace Persistence.DBFactories;

public class FastTicketsDBFactoryTest : FastTicketsDBFactoryBase
{
    public FastTicketsDBFactoryTest()
    {
        _dbName = DbName.TestDb;
        _seedData = new SeedTestData();
    }
}
