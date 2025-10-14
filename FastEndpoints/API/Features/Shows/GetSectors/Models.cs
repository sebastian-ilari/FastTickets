namespace API.Features.Shows.GetSectors;

public class Request
{
    public int ShowId { get; set; }
}

public class Response
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public long TotalSpots { get; set; }
    public long AvailableSpots { get; set; }
}
