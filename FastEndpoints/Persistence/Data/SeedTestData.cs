using Models;

namespace Persistence.Data;

public class SeedTestData : ISeedData
{
    public async Task Run(FastTicketsDB db)
    {
        var firstShow = Show.Create("Artist 01", "Show Name 01", "Venue 01", new DateTime(1990, 1, 1),
            id: TestDataIds.FIRST_SHOW_ID);
        var secondShow = Show.Create("Artist 02", "Show Name 02", "Venue 02", new DateTime(1995, 2, 1),
            id: TestDataIds.SECOND_SHOW_ID);

        var shows = new List<Show> {
            firstShow,
            secondShow
        };

        firstShow.AddSectors(
        [
            Sector.Create(firstShow.Id, "Sector 01", 100, 100, id: TestDataIds.SECOND_SHOW_SECTOR_ID)
        ]);
        secondShow.AddSectors(
        [
            Sector.Create(secondShow.Id, "Sector 01", 200, 100),
            Sector.Create(secondShow.Id, "Sector 02", 400, 200)
        ]);

        await db.Shows.AddRangeAsync(shows);

        var firstSectorId = firstShow.Sectors.First().Id;
        var ticket = Ticket.Create(firstShow.Id, firstSectorId, 10, id: TestDataIds.TICKET_ID);

        await db.Tickets.AddAsync(ticket);
        await db.SaveChangesAsync();
    }
}
