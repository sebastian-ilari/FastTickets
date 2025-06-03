namespace Models.Dtos;

public class ShowDto
{
    public int Id { get; set; }
    public string Artist { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }

    public ShowDto() { }
    public ShowDto(Show show) => (Id, Artist, Name, Venue, Date) = 
        (show.Id, show.Artist, show.Name, show.Venue, show.Date);
}
