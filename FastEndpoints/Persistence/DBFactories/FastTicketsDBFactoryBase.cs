using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Data;

namespace Persistence.DBFactories;

public abstract class FastTicketsDBFactoryBase
{
    protected FastTicketsDB _db = null!;
    protected string _dbName = string.Empty;
    protected ISeedData _seedData = null!;

    public async Task<FastTicketsDB> Create()
    {
        var options = new DbContextOptionsBuilder<FastTicketsDB>()
            .UseSqlite($"Data Source={_dbName};Mode=Memory;Cache=Shared")
            .Options;

        _db = new FastTicketsDB(options);
        await _db.Database.OpenConnectionAsync();
        await _db.Database.EnsureCreatedAsync();

        await _seedData.Run(_db);

        return _db;
    }

    /// <summary>
    /// Removes and reseeds data.
    /// ATTENTION: at this moment, this method DOES NOT reset the identity columns.
    /// </summary>
    public async Task ClearData()
    {
        var tables = _db.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();

        foreach (var table in tables)
        {
            await _db.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");
        }

        await _seedData.Run(_db);
    }

    public async Task Dispose()
    {
        await _db.Database.EnsureDeletedAsync();
        await _db.DisposeAsync();
    }
}
