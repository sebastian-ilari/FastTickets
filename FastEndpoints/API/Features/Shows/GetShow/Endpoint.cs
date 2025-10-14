using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Features.Shows.GetShow;

public class Endpoint : Endpoint<Request, Results<Ok<Response>, BadRequest<string>>, Mapper>
{
    private readonly FastTicketsDB _fastTicketsDB;

    public Endpoint(FastTicketsDB fastTicketsDB)
    {
        _fastTicketsDB = fastTicketsDB;
    }

    public override void Configure()
    {
        Get("show/{showId}");
        AllowAnonymous();
        Description(x => x.WithName("GetShow"));
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        var show = await _fastTicketsDB.Shows
            .FirstOrDefaultAsync(s => s.Id == request.ShowId, cancellationToken: cancellationToken);
        if (show == null)
        {
            ThrowError(r => r.ShowId, $"Show {request.ShowId} not found", StatusCodes.Status404NotFound);
            return;
        }

        await SendResultAsync(TypedResults.Ok(Map.FromEntity(show)));
    }
}
