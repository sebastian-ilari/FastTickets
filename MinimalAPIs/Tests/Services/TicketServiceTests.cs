using NUnit.Framework;
using Persistence.Data;
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
        Assert.That(() => _ticketService.BuyTicket(TestDataIds.FIRST_SHOW_ID, TestDataIds.INVALID_ID, 5),
            Throws.InvalidOperationException
                .With.Message.EqualTo($"Sector {TestDataIds.INVALID_ID} not found for show {TestDataIds.FIRST_SHOW_ID}")
        );
    }

    [Test]
    public void BuyTickets_ShowNotFound_ThrowsException()
    {
        Assert.That(() => _ticketService.BuyTicket(TestDataIds.INVALID_ID, TestDataIds.SECOND_SHOW_SECTOR_ID, 5),
            Throws.InvalidOperationException
                .With.Message.EqualTo($"Show {TestDataIds.INVALID_ID} not found")
        );
    }

    [Test]
    public async Task BuyTickets_NoErrors_TicketIsReturned()
    {
        var ticket = await _ticketService.BuyTicket(TestDataIds.SECOND_SHOW_ID, TestDataIds.SECOND_SHOW_SECTOR_ID, 5);

        Assert.That(ticket, Is.Not.Null);
        Assert.That(ticket.ShowId, Is.EqualTo(TestDataIds.SECOND_SHOW_ID));
        Assert.That(ticket.Show, Is.Not.Null);
        Assert.That(ticket.SectorId, Is.EqualTo(TestDataIds.SECOND_SHOW_SECTOR_ID));
        Assert.That(ticket.Sector, Is.Not.Null);
        Assert.That(ticket.Quantity, Is.EqualTo(5));
    }

    [Test]
    public async Task BuyTickets_NoErrors_SectorAvailableSpotsAreUpdated()
    {
        var availableSpots = _db.Sectors.First(s => s.Id == TestDataIds.SECOND_SHOW_SECTOR_ID).AvailableSpots;
        
        await _ticketService.BuyTicket(TestDataIds.SECOND_SHOW_ID, TestDataIds.SECOND_SHOW_SECTOR_ID, 5);

        Assert.That(_db.Sectors.First(s => s.Id == TestDataIds.SECOND_SHOW_SECTOR_ID).AvailableSpots, 
            Is.EqualTo(availableSpots - 5));
    }
}
