using Models;

namespace Persistence.Data;

public class SeedData : ISeedData
{
    public async Task Run(FastTicketsDB db)
    {
        var pinkFloydShow = Show.Create("Pink Floyd", "Live at Ponpeii", "Ancient Roman amphitheatre - Pompeii", 
            new DateTime(1971, 10, 1));
        var davidBowieShow = Show.Create("David Bowie", "Ziggy Stardust tour", "Borough Assembly Hall", 
            new DateTime(1972, 1, 29));
        var queenShow = Show.Create("Queen", "Live Aid", "Wembley Stadium", new DateTime(1985, 7, 13));
        var u2Show = Show.Create("U2", "Zoo TV", "Ireland RDS Arena", new DateTime(1993, 8, 28));

        var shows = new List<Show> {
            pinkFloydShow,
            davidBowieShow,
            queenShow,
            u2Show
        };

        await db.Shows.AddRangeAsync(shows);
        await db.SaveChangesAsync();

        pinkFloydShow.AddSectors(
        [
            Sector.Create(pinkFloydShow.Id, "Crew", 100, 100)
        ]);
        davidBowieShow.AddSectors(
        [
            Sector.Create(davidBowieShow.Id, "Standing", 5000, 5000),
            Sector.Create(davidBowieShow.Id, "Left side seating", 1000, 1000),
            Sector.Create(davidBowieShow.Id, "Right side seating", 1000, 1000),
            Sector.Create(davidBowieShow.Id, "Back seating", 2000, 2000),
        ]);
        queenShow.AddSectors(
        [
            Sector.Create(queenShow.Id, "Standing", 200000, 200000),
            Sector.Create(queenShow.Id, "First level seating", 3000, 3000),
            Sector.Create(queenShow.Id, "Second level seating", 15000, 15000)
        ]);
        u2Show.AddSectors(
        [
            Sector.Create(u2Show.Id, "General", 70000, 70000),
            Sector.Create(u2Show.Id, "VIP", 4000, 4000)
        ]);

        await db.SaveChangesAsync();
    }
}
