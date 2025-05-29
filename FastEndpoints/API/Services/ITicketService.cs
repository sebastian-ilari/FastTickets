using Models;

namespace API.Services;

public interface ITicketService
{
    public Task<Ticket> BuyTicket(int showId, int sectorId, int quantity);
}
