using Models;
using Persistence;

namespace API.Services;

public class TicketService : ITicketService
{
    private readonly FastTicketsDB _fastTicketsDB;

    public TicketService(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public async Task<Ticket> BuyTicket(int showId, int sectorId, int quantity)
    {
        var sector = await _fastTicketsDB.Sectors.FindAsync(sectorId)
            ?? throw new InvalidOperationException($"Sector {sectorId} not found for show {showId}");
        _ = await _fastTicketsDB.Shows.FindAsync(showId)
            ?? throw new InvalidOperationException($"Show {showId} not found");

        sector?.UpdateAvailableSpots(quantity);

        var ticket = Ticket.Create(showId, sectorId, quantity);
        _fastTicketsDB.Tickets.Add(ticket);

        await _fastTicketsDB.SaveChangesAsync();

        return ticket;
    }
}
