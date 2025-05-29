using Models.Output;
using Models.Request;
using NUnit.Framework;
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
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<ShowOutput>>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!, Has.Count.EqualTo(2));
        Assert.That(result!.ToArray()[0].Artist, Is.EqualTo("Artist 01"));
        Assert.That(result!.ToArray()[0].Name, Is.EqualTo("Show Name 01"));
        Assert.That(result!.ToArray()[0].Venue, Is.EqualTo("Venue 01"));
        Assert.That(result!.ToArray()[0].Date, Is.EqualTo(new DateTime(1990, 1, 1)));
    }

    [Test]
    public async Task GetAvailableTickets_ShowNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/fast-tickets/show/200/tickets");
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetAvailableTickets_ShowFound_ReturnsSectors()
    {
        var response = await _client.GetAsync("/fast-tickets/show/2/tickets");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<SectorOutput>>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!, Has.Count.EqualTo(2));
        Assert.That(result!.ToArray()[0].Name, Is.EqualTo("Sector 01"));
        Assert.That(result!.ToArray()[0].TotalSpots, Is.EqualTo(200));
        //Not asserting on this property because it will change when tests that buy tickets are run
        //Assert.That(result!.ToArray()[0].AvailableSpots, Is.EqualTo(100));
        Assert.That(result!.ToArray()[1].Name, Is.EqualTo("Sector 02"));
        Assert.That(result!.ToArray()[1].TotalSpots, Is.EqualTo(400));
        Assert.That(result!.ToArray()[1].AvailableSpots, Is.EqualTo(200));
    }

    [Test]
    public async Task BuyTicket_QuantityIsZero_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 0));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task BuyTicket_QuantityIsNegative_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, -5));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task BuyTicket_ShowNotFound_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/200/tickets", new BuyTicketRequest(2, 1));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task BuyTicket_SectorNotFound_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(200, 1));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task BuyTicket_NoTicketsAvailable_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 150));

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
    }

    [Test]
    public async Task BuyTicket_TicketsAvailable_ReturnsTicket()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 5));

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TicketOutput>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Show, Is.EqualTo("Show Name 02"));
        Assert.That(result!.Artist, Is.EqualTo("Artist 02"));
        Assert.That(result!.Sector, Is.EqualTo("Sector 01"));
        Assert.That(result!.Quantity, Is.EqualTo(5));
        Assert.That(result!.Date, Is.EqualTo(new DateTime(1995, 2, 1)));
    }

    [Test]
    public async Task GetTickets_TicketsAvailable_ReturnsTicket()
    {
        var response = await _client.GetAsync("/fast-tickets/tickets");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<TicketOutput>>();

        Assert.That(result, Is.Not.Null);
        Assert.That(result!, Has.Count.EqualTo(1));
        Assert.That(result!.First().Show, Is.EqualTo("Show Name 02"));
        Assert.That(result!.First().Artist, Is.EqualTo("Artist 02"));
        Assert.That(result!.First().Sector, Is.EqualTo("Sector 01"));
        Assert.That(result!.First().Quantity, Is.EqualTo(5));
        Assert.That(result!.First().Date, Is.EqualTo(new DateTime(1995, 2, 1)));
    }
}
