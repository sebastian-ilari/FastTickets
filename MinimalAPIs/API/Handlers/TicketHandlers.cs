using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Persistence;
using Services;

namespace Api.Handlers;

internal static class TicketHandlers
{
    internal static async Task<IResult> GetAvailableTickets(FastTicketsDB db, int showId)
    {
        var show = await db.Shows.FindAsync(showId);
        if (show == null)
        {
            return TypedResults.NotFound($"Show {showId} not found");
        }

        return TypedResults.Ok(await db.Sectors.Where(s => s.ShowId == showId)
            .Select(s => new SectorDto(s)).ToListAsync());
    }

    internal static async Task<IResult> BuyTicket(ITicketService ticketService, int showId, TicketForCreationDto ticketForCreationDto)
    {
        if (ticketForCreationDto.Quantity <= 0)
        {
            return TypedResults.BadRequest("Quantity must be greater than 0");
        }

        var ticket = await ticketService.BuyTicket(showId, ticketForCreationDto.SectorId, ticketForCreationDto.Quantity);
        var ticketDto = new TicketDto(ticket);

        return TypedResults.CreatedAtRoute(ticketDto, "GetTicket", new { ticketId = ticket.Id });
    }

    internal static async Task<IResult> GetTickets(FastTicketsDB db) => 
        TypedResults.Ok(await db.Tickets
            .Include(t => t.Show)
            .Include(t => t.Sector)
            .Select(t => new TicketDto(t))
            .ToListAsync());

    internal static async Task<IResult> GetTicket(FastTicketsDB db, int ticketId)
    {
        var ticket = await db.Tickets
            .Include(t => t.Show)
            .Include(t => t.Sector)
            .FirstOrDefaultAsync(t => t.Id == ticketId);

        if (ticket == null)
        {
            return TypedResults.NotFound($"Ticket {ticketId} not found");
        }
        else
        {
            return TypedResults.Ok(new TicketDto(ticket));
        }
    }
}
