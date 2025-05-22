namespace Models;

public class Sector
{
    public int Id { get; private set; }
    public int ShowId { get; private set; }
    public string Name { get; private set; } = null!;
    public long TotalSpots { get; private set; }
    public long AvailableSpots { get; private set; }

    public static Sector Create(int showId, string name, long totalSpots, long availableSpots) => new()
    {
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
