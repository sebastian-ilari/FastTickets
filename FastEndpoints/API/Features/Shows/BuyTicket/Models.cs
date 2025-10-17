namespace API.Features.Shows.BuyTicket;

public class Request(Guid showId, Guid sectorId, int quantity)
{
    public Guid ShowId { get; set; } = showId;
    public Guid SectorId { get; set; } = sectorId;
    public int Quantity { get; set; } = quantity;
}

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(r => r.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}

public class Response
{
    public Guid Id { get; set; }
    public string Show { get; set; } = null!;
    public string Artist { get; set; } = null!;
    public string Sector { get; set; } = null!;
    public int Quantity { get; set; }
    public string Venue { get; set; } = null!;
    public DateTime Date { get; set; }
}
