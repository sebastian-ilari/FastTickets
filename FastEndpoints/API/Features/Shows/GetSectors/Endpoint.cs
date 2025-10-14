using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Features.Shows.GetSectors;

public class Endpoint : Endpoint<Request, Results<Ok<List<Response>>, BadRequest<string>>, Mapper>
{
    private readonly FastTicketsDB _fastTicketsDB;

    public Endpoint(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public override void Configure()
    {
        Get("show/{ShowId}/tickets");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var show = await _fastTicketsDB.Shows.FindAsync(request.ShowId, cancellationToken);
        if (show == null)
        {
            ThrowError(r => r.ShowId, $"Show {request.ShowId} not found", StatusCodes.Status404NotFound);
        }

        await SendResultAsync(TypedResults.Ok(await _fastTicketsDB.Sectors
            .Where(s => s.ShowId == request.ShowId)
            .Select(s => Map.FromEntity(s))
            .ToListAsync(cancellationToken: cancellationToken)));
    }
}
