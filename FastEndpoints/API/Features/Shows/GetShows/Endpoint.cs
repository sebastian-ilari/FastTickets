using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Features.Shows.GetShows;

public class Endpoint : EndpointWithoutRequest<List<Response>, Mapper>
{
    private readonly FastTicketsDB _fastTicketsDB;

    public Endpoint(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public override void Configure()
    {
        Get("shows");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        var response = await _fastTicketsDB.Shows
            .Select(s => Map.FromEntity(s))
            .ToListAsync(cancellationToken: cancellationToken);

        await SendOkAsync(response);
    }
}
