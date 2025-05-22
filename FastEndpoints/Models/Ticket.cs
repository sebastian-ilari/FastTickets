namespace Models;

public class Ticket
{
    public int Id { get; private set; }
    public int ShowId { get; private set; }
    public virtual Show? Show { get; private set; }
    public int SectorId { get; private set; }
    public virtual Sector? Sector { get; private set; }
    public int Quantity { get; private set; }

    public static Ticket Create(int showId, int sectorId, int quantity) => new()
    {
        ShowId = showId,
        SectorId = sectorId,
        Quantity = quantity
    };
}
