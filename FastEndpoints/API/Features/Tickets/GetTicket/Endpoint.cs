using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Features.Tickets.GetTicket;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, BadRequest<string>>, Mapper>
{
    private readonly FastTicketsDB _fastTicketsDB;

    public Endpoint(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public override void Configure()
    {
        Get("ticket/{ticketId}");
        AllowAnonymous();
        Description(x => x.WithName("GetTicket"));
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var ticket = await _fastTicketsDB.Tickets
            .Include(t => t.Show)
            .Include(t => t.Sector)
            .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken: cancellationToken);
        if (ticket == null)
        {
            ThrowError(r => r.TicketId, $"Ticket {request.TicketId} not found", StatusCodes.Status404NotFound);
            return;
        }

        await SendResultAsync(TypedResults.Ok(Map.FromEntity(ticket)));
    }
}
