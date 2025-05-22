using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Features.Shows.GetShows;

internal sealed class Endpoint : EndpointWithoutRequest<List<Response>>
{
    private readonly FastTicketsDB _fastTicketsDB;

    public Endpoint(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public override void Configure()
    {
        Get("/fast-tickets/shows");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var response = await _fastTicketsDB.Shows
            .Select(s => new Response(s))
            .ToListAsync(cancellationToken: cancellationToken);

        await SendOkAsync(response);
    }
}
