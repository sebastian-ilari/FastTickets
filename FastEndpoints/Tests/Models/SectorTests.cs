using Models;
using NUnit.Framework;
using Shouldly;

namespace Tests.Models;

[TestFixture]
public class SectorTests
{
    [Test]
    public void UpdateAvailableSpots_NotEnoughSpots_ThrowsException()
    {
        var sector = Sector.Create(Guid.NewGuid(), "Test", 10, 5);

        var action = () => sector.UpdateAvailableSpots(50);
        
        action.ShouldThrow<InvalidOperationException>("Not enough available spots on Sector Test");
    }

    [Test]
    public void UpdateAvailableSpots_EnoughSpots_AvailableSpotsAreUpdated()
    {
        var sector = Sector.Create(Guid.NewGuid(), "Test", 10, 50);

        sector.UpdateAvailableSpots(5);

        sector.AvailableSpots.ShouldBe(45);
    }
}
