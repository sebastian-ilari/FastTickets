namespace Models.Dtos;

public class TicketDto
{
    public Guid Id { get; set; }
    public string Show { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public int Quantity { get; set; }
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }

    public TicketDto() { }
    public TicketDto(Ticket ticket) => (Id, Show, Artist, Sector, Quantity, Venue, Date) = 
        (ticket.Id, ticket.Show?.Name ?? "", ticket.Show?.Artist ?? "", ticket.Sector?.Name ?? "", ticket.Quantity, 
            ticket.Show?.Venue ?? "", ticket.Show?.Date ?? DateTime.MinValue);
}
