namespace API.Features.Shows.GetShow;

public class Request
{
    public int ShowId { get; set; }
}

public class Response
{
    public int Id { get; set; }
    public string Artist { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }
}
