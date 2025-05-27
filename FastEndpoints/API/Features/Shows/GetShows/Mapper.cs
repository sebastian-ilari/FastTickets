using Models;

namespace API.Features.Shows.GetShows;

public class Mapper : Mapper<object, GetShowsResponse, Show>
{
    public override GetShowsResponse FromEntity(Show show) => new()
    {
        Id = show.Id,
        Artist = show.Artist,
        Name = show.Name,
        Venue = show.Venue,
        Date = show.Date
    };
}
