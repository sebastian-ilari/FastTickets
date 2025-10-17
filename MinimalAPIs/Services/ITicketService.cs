using Models;

namespace Services;

public interface ITicketService
{
    public Task<Ticket> BuyTicket(Guid showId, Guid sectorId, int quantity);
}
