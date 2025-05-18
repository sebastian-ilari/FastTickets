using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Persistence;
using Persistence.DBFactories;
using Tests.Infrastructure;

namespace Tests.Api;

[TestFixture]
public abstract class APITestsBase
{
    protected HttpClient _client = null!;
    protected FastTicketsDB _db = null!;
    private WebApplicationFactory<Program> _factory = null!;
    private FastTicketsDBFactoryBase _dbFactory = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _factory = new TestingApplication();
        _client = _factory.CreateClient();
        _dbFactory = new FastTicketsDBFactoryTest();
        _db = await _dbFactory.Create();
    }

    /*
     * Commenting this because clearing the data but not reseting the identity columns brings issues with tests.
    [TearDown]
    public async Task TearDown()
    {
        await _dbFactory.ClearData();
    }
    */

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        await _dbFactory.Dispose();
    }
}
