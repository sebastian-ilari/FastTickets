using Api.Handlers;

namespace API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterShowEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/shows", ShowHandlers.GetShows);
        app.MapGet("/show/{showId:int}", ShowHandlers.GetShowById);
    }

    public static void RegisterTicketEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/show/{showId:int}/tickets", TicketHandlers.GetSectors);
        app.MapPost("/show/{showId:int}/tickets", TicketHandlers.BuyTicket);
        app.MapGet("/tickets", TicketHandlers.GetTickets);
        app.MapGet("/ticket/{ticketId:int}", TicketHandlers.GetTicket).WithName("GetTicket");
    }
}
