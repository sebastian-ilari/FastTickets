using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Models.Request;
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

        return group;
    }

    static async Task<IResult> GetShows(FastTicketsDB db)
    {
        return TypedResults.Ok(await db.Shows.Select(s => new ShowDto(s)).ToListAsync());
    }

    static async Task<IResult> GetAvailableTickets(int showId, FastTicketsDB db)
    {
        var show = await db.Shows.FindAsync(showId);
        if (show == null)
        {
            return TypedResults.NotFound($"Show {showId} not found");
        }

        return TypedResults.Ok(await db.Sectors.Where(s => s.ShowId == showId)
            .Select(s => new SectorDto(s)).ToListAsync());
    }

    static async Task<IResult> BuyTicket(int showId, BuyTicketRequest request, ITicketService ticketService)
    {
        if (request.Quantity <= 0)
        {
            return TypedResults.BadRequest("Quantity must be greater than 0");
        }

        var ticket = await ticketService.BuyTicket(showId, request.SectorId, request.Quantity);
        var ticketDto = new TicketDto(ticket);

        return TypedResults.Created($"/fast-tickets/show/{showId}/tickets", ticketDto);
    }

    static async Task<IResult> GetTickets(FastTicketsDB db) => 
        TypedResults.Ok(await db.Tickets
            .Include(t => t.Show)
            .Include(t => t.Sector)
            .Select(t => new TicketDto(t))
            .ToListAsync());
}
