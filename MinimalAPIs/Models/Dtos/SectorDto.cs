namespace Models.Dtos;

public class SectorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public long TotalSpots { get; set; }
    public long AvailableSpots { get; set; }

    public SectorDto() { }
    public SectorDto(Sector sector) => (Id, Name, TotalSpots, AvailableSpots) =
        (sector.Id, sector.Name, sector.TotalSpots, sector.AvailableSpots);
}
