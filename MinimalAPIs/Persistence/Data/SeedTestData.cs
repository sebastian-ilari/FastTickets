using Models;

namespace Persistence.Data;

public class SeedTestData : ISeedData
{
    public async Task Run(FastTicketsDB db)
    {
        var firstShow = Show.Create("Artist 01", "Show Name 01", "Venue 01", new DateTime(1990, 1, 1));
        var secondShow = Show.Create("Artist 02", "Show Name 02", "Venue 02", new DateTime(1995, 2, 1));

        var shows = new List<Show> {
            firstShow,
            secondShow
        };

        await db.Shows.AddRangeAsync(shows);
        await db.SaveChangesAsync();

        firstShow.AddSectors(
        [
            Sector.Create(firstShow.Id, "Sector 01", 100, 100)
        ]);
        secondShow.AddSectors(
        [
            Sector.Create(secondShow.Id, "Sector 01", 200, 100),
            Sector.Create(secondShow.Id, "Sector 02", 400, 200)
        ]);

        await db.SaveChangesAsync();
    }
}
