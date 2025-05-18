using NUnit.Framework;
using Services;

namespace Tests.Services;

[TestFixture]
public class TicketServiceTests : ServiceTestsBase
{
    private ITicketService _ticketService = null!;

    [OneTimeSetUp]
    public new void OneTimeSetup()
    {
        _ticketService = new TicketService(_db);
    }

    [Test]
    public void BuyTickets_SectorNotFound_ThrowsException()
    {
        Assert.That(() => _ticketService.BuyTicket(1, 1000, 5),
            Throws.InvalidOperationException
                .With.Message.EqualTo("Sector 1000 not found for show 1")
        );
    }

    [Test]
    public void BuyTickets_ShowNotFound_ThrowsException()
    {
        Assert.That(() => _ticketService.BuyTicket(1000, 1, 5),
            Throws.InvalidOperationException
                .With.Message.EqualTo("Show 1000 not found")
        );
    }

    [Test]
    public async Task BuyTickets_NoErrors_TicketIsReturned()
    {
        var ticket = await _ticketService.BuyTicket(1, 1, 5);

        Assert.That(ticket, Is.Not.Null);
        Assert.That(ticket.Id, Is.GreaterThan(0));
        Assert.That(ticket.ShowId, Is.EqualTo(1));
        Assert.That(ticket.Show, Is.Not.Null);
        Assert.That(ticket.SectorId, Is.EqualTo(1));
        Assert.That(ticket.Sector, Is.Not.Null);
        Assert.That(ticket.Quantity, Is.EqualTo(5));
    }

    [Test]
    public async Task BuyTickets_NoErrors_SectorAvailableSpotsAreUpdated()
    {
        var availableSpots = _db.Sectors.First(s => s.Id == 1).AvailableSpots;
        
        await _ticketService.BuyTicket(1, 1, 5);

        Assert.That(_db.Sectors.First(s => s.Id == 1).AvailableSpots, Is.EqualTo(availableSpots - 5));
    }
}
