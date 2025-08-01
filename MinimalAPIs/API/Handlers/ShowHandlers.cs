using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;
using Persistence;

namespace Api.Handlers;

internal static class ShowHandlers
{
     internal static async Task<Ok<List<ShowDto>>> GetShows(FastTicketsDB db) => 
        TypedResults.Ok(await db.Shows.Select(s => new ShowDto(s)).ToListAsync());
}
