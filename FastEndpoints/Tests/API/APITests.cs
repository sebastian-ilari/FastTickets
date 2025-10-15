using NUnit.Framework;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using BuyTicketResponse = API.Features.Shows.BuyTicket.Response;
using BuyTicketRequest = API.Features.Shows.BuyTicket.Request;
using GetSectorsResponse = API.Features.Shows.GetSectors.Response;
using GetShowsResponse = API.Features.Shows.GetShows.Response;
using GetShowResponse = API.Features.Shows.GetShow.Response;
using GetTicketsResponse = API.Features.Tickets.GetTickets.Response;

namespace Tests.API;

[TestFixture]
public class APITests : APITestsBase
{
    [Test]
    public async Task GetShows_ReturnsShows()
    {
        var response = await _client.GetAsync("/fast-tickets/shows");
        
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<GetShowsResponse>>();

        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result!.ToArray()[0].Artist.ShouldBe("Artist 01");
        result!.ToArray()[0].Name.ShouldBe("Show Name 01");
        result!.ToArray()[0].Venue.ShouldBe("Venue 01");
        result!.ToArray()[0].Date.ShouldBeEquivalentTo(new DateTime(1990, 1, 1));
    }

    [Test]
    public async Task GetShow_ShowIsNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/fast-tickets/show/5000");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetShow_ShowIsFound_ReturnsShow()
    {
        var response = await _client.GetAsync("/fast-tickets/show/1");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GetShowResponse>();

        result.ShouldNotBeNull();
        result!.Artist.ShouldBe("Artist 01");
        result!.Name.ShouldBe("Show Name 01");
        result!.Venue.ShouldBe("Venue 01");
        result!.Date.ShouldBeEquivalentTo(new DateTime(1990, 1, 1));
    }

    [Test]
    public async Task GetSectors_ShowNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/fast-tickets/show/200/tickets");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetSectors_ShowFound_ReturnsSectors()
    {
        var response = await _client.GetAsync("/fast-tickets/show/2/tickets");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetSectorsResponse>>();

        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result!.ToArray()[0].Name.ShouldBe("Sector 01");
        result!.ToArray()[0].TotalSpots.ShouldBe(200);
        result!.ToArray()[0].AvailableSpots.ShouldBe(100);
        result!.ToArray()[1].Name.ShouldBe("Sector 02");
        result!.ToArray()[1].TotalSpots.ShouldBe(400);
        result!.ToArray()[1].AvailableSpots.ShouldBe(200);
    }

    [Test]
    public async Task GetTickets_TicketsAvailable_ReturnsTicket()
    {
        var response = await _client.GetAsync("/fast-tickets/tickets");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetTicketsResponse>>();

        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.First().Show.ShouldBe("Show Name 01");
        result.First().Artist.ShouldBe("Artist 01");
        result.First().Sector.ShouldBe("Sector 01");
        result.First().Quantity.ShouldBe(10);
        result.First().Date.ShouldBeEquivalentTo(new DateTime(1990, 1, 1));
    }

    [Test]
    public async Task GetTicket_TicketIsNotFound_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/fast-tickets/ticket/5000");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetTicket_TicketIsFound_ReturnsTicket()
    {
        var response = await _client.GetAsync("/fast-tickets/ticket/1");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GetTicketsResponse>();

        result.ShouldNotBeNull();
        result.Show.ShouldBe("Show Name 01");
        result.Artist.ShouldBe("Artist 01");
        result.Sector.ShouldBe("Sector 01");
        result.Quantity.ShouldBe(10);
        result.Date.ShouldBeEquivalentTo(new DateTime(1990, 1, 1));
    }

    [Test]
    public async Task BuyTicket_QuantityIsZero_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 2, 0));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task BuyTicket_QuantityIsNegative_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 2, -5));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task BuyTicket_ShowNotFound_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/200/tickets", new BuyTicketRequest(200, 2, 1));

        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }

    [Test]
    public async Task BuyTicket_SectorNotFound_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 200, 1));

        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }

    [Test]
    public async Task BuyTicket_NoTicketsAvailable_ReturnsInternalServerError()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 2, 150));

        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }

    [Test]
    public async Task BuyTicket_TicketsAvailable_ReturnsTicket()
    {
        var response = await _client.PostAsJsonAsync("/fast-tickets/show/2/tickets", new BuyTicketRequest(2, 2, 5));

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<BuyTicketResponse>();

        result.ShouldNotBeNull();
        result.Show.ShouldBe("Show Name 02");
        result.Artist.ShouldBe("Artist 02");
        result.Sector.ShouldBe("Sector 01");
        result.Quantity.ShouldBe(5);
        result.Date.ShouldBeEquivalentTo(new DateTime(1995, 2, 1));
    }
}
