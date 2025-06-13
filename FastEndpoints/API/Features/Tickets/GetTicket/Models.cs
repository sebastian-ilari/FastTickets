namespace API.Features.Tickets.GetTicket;

public class Request
{
    public int TicketId { get; set; }
}

public class Response
{
    public int Id { get; set; }
    public string Show { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public int Quantity { get; set; }
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }
}
