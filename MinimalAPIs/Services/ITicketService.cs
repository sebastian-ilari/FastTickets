using Models;

namespace Services;

public interface ITicketService
{
    public Task<Ticket> BuyTicket(int showId, int sectorId, int quantity);
}
