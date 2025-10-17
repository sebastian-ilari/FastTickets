namespace Models;

public class Ticket
{
    public Guid Id { get; private set; }
    public Guid ShowId { get; private set; }
    public virtual Show? Show { get; private set; }
    public Guid SectorId { get; private set; }
    public virtual Sector? Sector { get; private set; }
    public int Quantity { get; private set; }

    public static Ticket Create(Guid showId, Guid sectorId, int quantity, Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        ShowId = showId,
        SectorId = sectorId,
        Quantity = quantity
    };
}
