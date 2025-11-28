namespace MartenEventSourcing.Web.Tickets.Commands;

public record SelectResolution(Guid TicketId, string Resolution);