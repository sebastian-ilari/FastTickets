using Models;
using NUnit.Framework;

namespace Tests.Models;

[TestFixture]
public class SectorTests
{
    [Test]
    public void UpdateAvailableSpots_NotEnoughSpots_ThrowsException()
    {
        var sector = Sector.Create(1, "Test", 10, 5);

        Assert.That(() => sector.UpdateAvailableSpots(50),
            Throws.InvalidOperationException
                .With.Message.EqualTo("Not enough available spots on Sector Test")
        );
    }

    [Test]
    public void UpdateAvailableSpots_EnoughSpots_AvailableSpotsAreUpdated()
    {
        var sector = Sector.Create(1, "Test", 10, 50);

        sector.UpdateAvailableSpots(5);

        Assert.That(sector.AvailableSpots, Is.EqualTo(45));
    }
}
