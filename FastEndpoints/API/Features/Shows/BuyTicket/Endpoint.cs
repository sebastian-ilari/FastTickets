using API.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Features.Shows.BuyTicket;

public class Endpoint : Endpoint<BuyTicketRequest, Results<Created<BuyTicketResponse>, BadRequest<string>>, Mapper>
{
    private readonly ITicketService _ticketService;

    public Endpoint(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public override void Configure()
    {
        Post("show/{ShowId}/tickets");
        AllowAnonymous();
    }

    public override async Task HandleAsync(BuyTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.BuyTicket(request.ShowId, request.SectorId, request.Quantity);

        await SendResultAsync(TypedResults.Created($"/fast-tickets/show/{request.ShowId}/tickets", Map.FromEntity(ticket)));
    }
}
