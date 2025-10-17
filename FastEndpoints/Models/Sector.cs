namespace Models;

public class Sector
{
    public Guid Id { get; private set; }
    public Guid ShowId { get; private set; }
    public string Name { get; private set; } = null!;
    public long TotalSpots { get; private set; }
    public long AvailableSpots { get; private set; }

    public static Sector Create(Guid showId, string name, long totalSpots, long availableSpots, Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        ShowId = showId,
        Name = name,
        TotalSpots = totalSpots,
        AvailableSpots = availableSpots
    };

    public void UpdateAvailableSpots(long quantity)
    {
        if (AvailableSpots - quantity < 0)
            throw new InvalidOperationException($"Not enough available spots on Sector {Name}");
        AvailableSpots -= quantity;
    }
}
