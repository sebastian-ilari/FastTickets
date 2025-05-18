namespace Persistence.Data;

public interface ISeedData
{
    public Task Run(FastTicketsDB db);
}
