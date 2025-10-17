using Models;
using Persistence;

namespace Services;

public class TicketService : ITicketService
{
    private readonly FastTicketsDB _db;

    public TicketService(FastTicketsDB db)
    {
        _db = db;
    }

    public async Task<Ticket> BuyTicket(Guid showId, Guid sectorId, int quantity)
    {
        var sector = await _db.Sectors.FindAsync(sectorId) 
            ?? throw new InvalidOperationException($"Sector {sectorId} not found for show {showId}");
        _ = await _db.Shows.FindAsync(showId)
            ?? throw new InvalidOperationException($"Show {showId} not found");

        sector?.UpdateAvailableSpots(quantity);

        var ticket = Ticket.Create(showId, sectorId, quantity);
        _db.Tickets.Add(ticket);

        await _db.SaveChangesAsync();

        return ticket;
    }
}
