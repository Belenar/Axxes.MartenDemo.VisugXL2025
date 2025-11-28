using JasperFx.Events;
using MartenEventSourcing.Web.Tickets.Events;


namespace MartenEventSourcing.Web.Tickets;

public record Ticket(
    Guid Id,
    string Title,
    string Description,
    string[] Comments,
    string? Resolution,
    bool IsClosed)
{
    public static Ticket Create(IEvent<TicketLogged> logged)
        => new(logged.StreamId, logged.Data.Title, logged.Data.Description, [], null, false);

    public static Ticket Apply(ResolutionProposed proposed, Ticket previous)
        => previous with { Comments = previous.Comments.Append(proposed.Comment).ToArray() };

    public static Ticket Apply(ResolutionSelected selected, Ticket previous)
        => previous with { Resolution = selected.Resolution };

    public static Ticket Apply(TicketClosed closed, Ticket previous)
        => previous with { IsClosed = true };
}
