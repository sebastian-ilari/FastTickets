using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Persistence;

namespace Api.Handlers;

internal static class ShowHandlers
{
     internal static async Task<Ok<List<ShowDto>>> GetShows(FastTicketsDB db) => 
        TypedResults.Ok(await db.Shows.Select(s => new ShowDto(s)).ToListAsync());

    internal static async Task<Results<NotFound<string>, Ok<ShowDto>>> GetShowById(FastTicketsDB db, Guid showId)
    {
        var show = await db.Shows.Where(s => s.Id == showId).FirstOrDefaultAsync();
        if (show == null)
        {
            return TypedResults.NotFound($"Show {showId} not found");
        }

        return TypedResults.Ok(new ShowDto(show));
    }
}
