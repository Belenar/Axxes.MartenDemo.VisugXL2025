namespace MartenEventSourcing.Web.Tickets.Commands;

public record ProposeResolution(Guid TicketId, string Comment);