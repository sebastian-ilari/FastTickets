namespace API.Features.Shows.GetShows;

public class Response
{
    public Guid Id { get; set; }
    public string Artist { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }
}
