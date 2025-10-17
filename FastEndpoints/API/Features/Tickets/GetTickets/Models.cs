namespace API.Features.Tickets.GetTickets;

public class Response
{
    public Guid Id { get; set; }
    public string Show { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public int Quantity { get; set; }
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }
}
