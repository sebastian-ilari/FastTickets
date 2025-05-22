using Models;

namespace API.Features.Shows.GetAvailableTickets;

internal sealed class Request
{
    public int ShowId { get; set; }
}

public sealed class Response
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public long TotalSpots { get; set; }
    public long AvailableSpots { get; set; }

    public Response() { }

    public Response(Sector sector) => (Id, Name, TotalSpots, AvailableSpots) =
        (sector.Id, sector.Name, sector.TotalSpots, sector.AvailableSpots);
}
