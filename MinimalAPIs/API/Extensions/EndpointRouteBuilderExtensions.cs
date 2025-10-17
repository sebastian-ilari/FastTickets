using Api.Handlers;

namespace API.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static void RegisterShowEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/shows", ShowHandlers.GetShows);
        app.MapGet("/show/{showId:Guid}", ShowHandlers.GetShowById);
    }

    public static void RegisterTicketEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/show/{showId:Guid}/sectors", TicketHandlers.GetSectors);
        app.MapPost("/show/{showId:Guid}/tickets", TicketHandlers.BuyTickets);
        app.MapGet("/tickets", TicketHandlers.GetTickets);
        app.MapGet("/ticket/{ticketId:Guid}", TicketHandlers.GetTicket).WithName("GetTicket");
    }
}
