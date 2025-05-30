using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Features.Tickets.GetTickets;

public class Endpoint : EndpointWithoutRequest<List<Response>, Mapper>
{
    private readonly FastTicketsDB _fastTicketsDB;

    public Endpoint(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public override void Configure()
    {
        Get("tickets");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var response = await _fastTicketsDB.Tickets
            .Include(t => t.Show)
            .Include(t => t.Sector)
            .Select(s => Map.FromEntity(s))
            .ToListAsync(cancellationToken: cancellationToken);

        await SendOkAsync(response);
    }
}
