using Models;

namespace API.Features.Tickets.GetTickets;

public class Mapper : Mapper<object, Response, Ticket>
{
    public override Response FromEntity(Ticket ticket) => new()
    {
        Id = ticket.Id,
        Show = ticket.Show?.Name ?? string.Empty,
        Artist = ticket.Show?.Artist ?? string.Empty,
        Sector = ticket.Sector?.Name ?? string.Empty,
        Quantity = ticket.Quantity,
        Venue = ticket.Show?.Venue ?? string.Empty,
        Date = ticket.Show?.Date ?? DateTime.MinValue
    };
}
