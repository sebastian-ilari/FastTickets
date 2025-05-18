using NUnit.Framework;
using Persistence;
using Persistence.DBFactories;
using Tests.Common;

namespace Tests.Services;

[TestFixture]
public abstract class ServiceTestsBase
{
    protected FastTicketsDB _db = null!;
    private FastTicketsDBFactoryBase _dbFactory = null!;

    [OneTimeSetUp]
    public virtual async Task OneTimeSetup()
    {
        _dbFactory = new FastTicketsDBFactoryTest(ConfigurationHelper.GetConfiguration());
        _db = await _dbFactory.Create();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _dbFactory.Dispose();
    }
}
