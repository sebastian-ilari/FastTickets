namespace Models.Output;

public class ShowOutput
{
    public int Id { get; set; }
    public string Artist { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }

    public ShowOutput() { }
    public ShowOutput(Show show) => (Id, Artist, Name, Venue, Date) = 
        (show.Id, show.Artist, show.Name, show.Venue, show.Date);
}
