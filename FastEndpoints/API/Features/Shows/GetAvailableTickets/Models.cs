namespace API.Features.Shows.GetAvailableTickets;

public class Request
{
    public int ShowId { get; set; }
}

public class GetAvailableTicketsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public long TotalSpots { get; set; }
    public long AvailableSpots { get; set; }
}
