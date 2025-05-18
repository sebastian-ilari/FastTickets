namespace Models;

public class Show
{
    public int Id { get; private set; }
    public string Artist { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Venue { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public virtual IEnumerable<Sector> Sectors { get; private set; } = null!;

    public static Show Create(string artist, string name, string venue, DateTime date) => new()
    {
        Artist = artist,
        Name = name,
        Venue = venue,
        Date = date
    };

    public void AddSectors(IEnumerable<Sector> sectors)
    {
        Sectors = sectors;
    }
}
