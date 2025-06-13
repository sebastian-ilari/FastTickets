using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Persistence;
using Services;

namespace Api.Endpoints;

public static class FastTicketsEndpoints
{
    public static void RegisterFastTicketsEndpoints(this WebApplication app)
    {
        app.MapGroup("/fast-tickets").MapEndpoints();
    }

    static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/shows", GetShows);
        group.MapGet("/show/{showId:int}/tickets", GetAvailableTickets);
        group.MapPost("/show/{showId:int}/tickets", BuyTicket);
        group.MapGet("/tickets", GetTickets);
        group.MapGet("/ticket/{ticketId:int}", GetTicket);

        return group;
    }

    static async Task<IResult> GetShows(FastTicketsDB db) => 
        TypedResults.Ok(await db.Shows.Select(s => new ShowDto(s)).ToListAsync());

    static async Task<IResult> GetAvailableTickets(FastTicketsDB db, int showId)
    {
        var show = await db.Shows.FindAsync(showId);
        if (show == null)
        {
            return TypedResults.NotFound($"Show {showId} not found");
        }

        return TypedResults.Ok(await db.Sectors.Where(s => s.ShowId == showId)
            .Select(s => new SectorDto(s)).ToListAsync());
    }

    static async Task<IResult> BuyTicket(ITicketService ticketService, int showId, TicketForCreationDto ticketForCreationDto)
    {
        if (ticketForCreationDto.Quantity <= 0)
        {
            return TypedResults.BadRequest("Quantity must be greater than 0");
        }

        var ticket = await ticketService.BuyTicket(showId, ticketForCreationDto.SectorId, ticketForCreationDto.Quantity);
        var ticketDto = new TicketDto(ticket);

        return TypedResults.Created($"/fast-tickets/show/{showId}/tickets", ticketDto);
    }

    static async Task<IResult> GetTickets(FastTicketsDB db) => 
        TypedResults.Ok(await db.Tickets
            .Include(t => t.Show)
            .Include(t => t.Sector)
            .Select(t => new TicketDto(t))
            .ToListAsync());

    static async Task<IResult> GetTicket(FastTicketsDB db, int ticketId)
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
