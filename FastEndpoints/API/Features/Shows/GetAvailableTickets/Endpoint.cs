using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Features.Shows.GetAvailableTickets;

internal sealed class Endpoint : Endpoint<Request, Results<Ok<List<Response>>, BadRequest<string>>>
{
    private readonly FastTicketsDB _fastTicketsDB;

    public Endpoint(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public override void Configure()
    {
        Get("/fast-tickets/show/{ShowId}/tickets");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var show = await _fastTicketsDB.Shows.FindAsync(request.ShowId, cancellationToken);
        if (show == null)
        {
            await SendResultAsync(TypedResults.BadRequest($"Show {request.ShowId} not found"));
            return;
        }

        await SendResultAsync(TypedResults.Ok(await _fastTicketsDB.Sectors
            .Where(s => s.ShowId == request.ShowId)
            .Select(s => new Response(s))
            .ToListAsync(cancellationToken: cancellationToken)));
    }
}
