using Models;

namespace API.Features.Shows.BuyTicket;

public class Mapper : Mapper<object, BuyTicketResponse, Ticket>
{
    public override BuyTicketResponse FromEntity(Ticket ticket) => new()
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
