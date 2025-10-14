using Models;

namespace API.Features.Shows.GetSectors;

public class Mapper : Mapper<object, Response, Sector>
{
    public override Response FromEntity(Sector sector) => new()
    {
        Id = sector.Id,
        Name = sector.Name,
        TotalSpots = sector.TotalSpots,
        AvailableSpots = sector.AvailableSpots
    };
}
