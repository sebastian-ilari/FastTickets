using Models.Dtos;
using NUnit.Framework;
using Persistence.Data;
using System.Net;
using System.Net.Http.Json;

namespace Tests.API;

[TestFixture]
public class APITests : APITestsBase
{
    [Test]
    public async Task GetShows_ReturnsShows()
    {
        var response = await _client.GetAsync("/fast-tickets/shows");
        
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ShowDto>>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!, Has.Count.EqualTo(2));
        Assert.That(result!.Any(s => s.Artist == "Artist 01" && s.Name == "Show Name 01" && s.Venue == "Venue 01" 
            && s.Date.Equals(new DateTime(1990, 1, 1))));
    }

    [Test]
    public async Task GetShowById_ShowIsNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/fast-tickets/show/{TestDataIds.INVALID_ID}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetShowById_ShowIsFound_ReturnsShow()
    {
        var response = await _client.GetAsync($"/fast-tickets/show/{TestDataIds.FIRST_SHOW_ID}");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ShowDto>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Artist, Is.EqualTo("Artist 01"));
        Assert.That(result!.Name, Is.EqualTo("Show Name 01"));
        Assert.That(result!.Venue, Is.EqualTo("Venue 01"));
        Assert.That(result!.Date, Is.EqualTo(new DateTime(1990, 1, 1)));
    }

    [Test]
    public async Task GetSectors_ShowNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/fast-tickets/show/{TestDataIds.INVALID_ID}/sectors");
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetSectors_ShowFound_ReturnsSectors()
    {
        var response = await _client.GetAsync($"/fast-tickets/show/{TestDataIds.SECOND_SHOW_ID}/sectors");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<SectorDto>>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!, Has.Count.EqualTo(2));
        Assert.That(result!.Any(s => s.Name == "Sector 01" && s.TotalSpots == 200 && s.AvailableSpots == 100));
        Assert.That(result!.Any(s => s.Name == "Sector 02" && s.TotalSpots == 400 && s.AvailableSpots == 200));
    }

    [Test]
    public async Task GetTickets_TicketsAvailable_ReturnsTicket()
    {
        var response = await _client.GetAsync("/fast-tickets/tickets");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<TicketDto>>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!, Has.Count.EqualTo(1));
        Assert.That(result!.First().Show, Is.EqualTo("Show Name 01"));
        Assert.That(result!.First().Artist, Is.EqualTo("Artist 01"));
        Assert.That(result!.First().Sector, Is.EqualTo("Sector 01"));
        Assert.That(result!.First().Quantity, Is.EqualTo(10));
        Assert.That(result!.First().Date, Is.EqualTo(new DateTime(1990, 1, 1)));
    }

    [Test]
    public async Task GetTicket_TicketIsNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/fast-tickets/ticket/{TestDataIds.INVALID_ID}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetTicket_TicketIsFound_ReturnsTicket()
    {
        var response = await _client.GetAsync($"/fast-tickets/ticket/{TestDataIds.TICKET_ID}");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TicketDto>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Show, Is.EqualTo("Show Name 01"));
        Assert.That(result.Artist, Is.EqualTo("Artist 01"));
        Assert.That(result.Sector, Is.EqualTo("Sector 01"));
        Assert.That(result.Quantity, Is.EqualTo(10));
        Assert.That(result.Date, Is.EqualTo(new DateTime(1990, 1, 1)));
    }

    [Test]
    public async Task BuyTicket_QuantityIsZero_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync($"/fast-tickets/show/{TestDataIds.SECOND_SHOW_ID}/tickets", 
            new TicketForCreationDto(TestDataIds.SECOND_SHOW_ID, 0));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task BuyTicket_QuantityIsNegative_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync($"/fast-tickets/show/{TestDataIds.SECOND_SHOW_ID}/tickets", 
            new TicketForCreationDto(TestDataIds.SECOND_SHOW_SECTOR_ID, -5));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task BuyTicket_ShowNotFound_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync($"/fast-tickets/show/{TestDataIds.INVALID_ID}/tickets", 
            new TicketForCreationDto(TestDataIds.INVALID_ID, 1));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task BuyTicket_SectorNotFound_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync($"/fast-tickets/show/{TestDataIds.SECOND_SHOW_ID}/tickets", 
            new TicketForCreationDto(TestDataIds.INVALID_ID, 1));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task BuyTicket_NoTicketsAvailable_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync($"/fast-tickets/show/{TestDataIds.SECOND_SHOW_ID}/tickets", 
            new TicketForCreationDto(TestDataIds.SECOND_SHOW_SECTOR_ID, 150));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task BuyTicket_TicketsAvailable_ReturnsTicket()
    {
        var response = await _client.PostAsJsonAsync($"/fast-tickets/show/{TestDataIds.SECOND_SHOW_ID}/tickets", 
            new TicketForCreationDto(TestDataIds.SECOND_SHOW_SECTOR_ID, 5));

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TicketDto>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Show, Is.EqualTo("Show Name 02"));
        Assert.That(result!.Artist, Is.EqualTo("Artist 02"));
        Assert.That(result!.Sector, Is.EqualTo("Sector 01"));
        Assert.That(result!.Quantity, Is.EqualTo(5));
        Assert.That(result!.Date, Is.EqualTo(new DateTime(1995, 2, 1)));
    }
}
