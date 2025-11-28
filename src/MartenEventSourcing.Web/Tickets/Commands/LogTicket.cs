namespace MartenEventSourcing.Web.Tickets.Commands;

public record LogTicket(Guid TicketId, string Title, string Description);