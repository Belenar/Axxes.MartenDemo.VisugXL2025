using JasperFx.Events;
using Marten.Events.Aggregation;
using MartenEventSourcing.Web.Tickets.Commands;
using MartenEventSourcing.Web.Tickets.Events;

namespace MartenEventSourcing.Web.Tickets.Projections;


public record OpenTicket(Guid Id, string Title, bool Resolved);

public class OpenTicketProjection : SingleStreamProjection<OpenTicket, Guid>
{
    public OpenTicketProjection()
    {
        DeleteEvent<TicketClosed>();
    }

    public OpenTicket Create(IEvent<LogTicket> logged)
        => new(logged.StreamId, logged.Data.Title, false);

    public OpenTicket Apply(ResolutionSelected resolved, OpenTicket previous)
        => previous with { Resolved = true };
}
