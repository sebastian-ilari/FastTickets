using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Persistence;
using Persistence.DBFactories;
using Tests.Common;
using Tests.Infrastructure;

namespace Tests.API;

[TestFixture]
public abstract class APITestsBase
{
    protected HttpClient _client = null!;
    protected FastTicketsDB _db = null!;
    private WebApplicationFactory<Program> _factory = null!;
    private FastTicketsDBFactoryBase _dbFactory = null!;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new TestingApplication();
        _client = _factory.CreateClient();
        _dbFactory = new FastTicketsDBFactoryTest(ConfigurationHelper.GetConfiguration());
    }

    [SetUp]
    public async Task SetUp()
    {
        _db = await _dbFactory.Create();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _dbFactory.ClearData();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        await _dbFactory.Dispose();
    }
}
