using Models;

namespace API.Features.Shows.GetAvailableTickets;

public class Mapper : Mapper<object, GetAvailableTicketsResponse, Sector>
{
    public override GetAvailableTicketsResponse FromEntity(Sector sector) => new()
    {
        Id = sector.Id,
        Name = sector.Name,
        TotalSpots = sector.TotalSpots,
        AvailableSpots = sector.AvailableSpots
    };
}
