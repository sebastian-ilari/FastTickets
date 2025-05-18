namespace Models.Output;

public class SectorOutput
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public long TotalSpots { get; set; }
    public long AvailableSpots { get; set; }

    public SectorOutput() { }
    public SectorOutput(Sector sector) => (Id, Name, TotalSpots, AvailableSpots) =
        (sector.Id, sector.Name, sector.TotalSpots, sector.AvailableSpots);
}
