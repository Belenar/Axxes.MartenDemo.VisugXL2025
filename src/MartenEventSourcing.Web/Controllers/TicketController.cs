using Marten;
using MartenEventSourcing.Web.Tickets;
using MartenEventSourcing.Web.Tickets.Commands;
using MartenEventSourcing.Web.Tickets.Events;
using Microsoft.AspNetCore.Mvc;
using System;

namespace MartenEventSourcing.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(
            [FromServices] IDocumentSession session, Guid id)
        {
            var ticket = await session.Events.FetchLatest<Ticket>(id);

            return ticket == null
                ? NotFound()
                : Ok(ticket);
        }

        [HttpGet("{id}/version/{version}")]
        public async Task<IActionResult> GetTicket(
            [FromServices] IDocumentSession session, Guid id, long version)
        {
            var ticket = await session.Events.AggregateStreamAsync<Ticket>(id, version: version);

            return ticket == null
                ? NotFound()
                : Ok(ticket);
        }

        [HttpGet("{id}/timestamp/{timestamp}")]
        public async Task<IActionResult> GetTicket(
            [FromServices] IDocumentSession session, Guid id, DateTimeOffset timestamp)
        {
            var ticket = await session.Events.AggregateStreamAsync<Ticket>(id, timestamp: timestamp);

            return ticket == null
                ? NotFound()
                : Ok(ticket);
        }


        [HttpPost("log")]
        public async Task<IActionResult> LogTicket(
            [FromServices] IDocumentSession session,
            [FromBody] LogTicket command)
        {
            var startEvent = new TicketLogged(command.Title, command.Description);

            session.Events.StartStream<Ticket>(command.TicketId, startEvent);

            await session.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("propose-resolution")]
        public async Task<IActionResult> ProposeResolution(
            [FromServices] IDocumentSession session,
            [FromBody] ProposeResolution command)
        {
            var stream = await session.Events.FetchForWriting<Ticket>(command.TicketId);
            var ticket = stream.Aggregate;

            if (ticket != null)
                stream.AppendOne(new ResolutionProposed(command.Comment));

            await session.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("select-resolution")]
        public async Task<IActionResult> SelectResolution(
            [FromServices] IDocumentSession session,
            [FromBody] SelectResolution command)
        {
            var stream = await session.Events.FetchForWriting<Ticket>(command.TicketId);
            var ticket = stream.Aggregate;

            if (ticket != null && ticket.Comments.Contains(command.Resolution))
                stream.AppendOne(new ResolutionSelected(command.Resolution));

            await session.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("close")]
        public async Task<IActionResult> CloseTicket(
            [FromServices] IDocumentSession session,
            [FromBody] CloseTicket command)
        {
            var stream = await session.Events.FetchForWriting<Ticket>(command.TicketId);
            var ticket = stream.Aggregate;

            if (ticket?.Resolution is not null)
                stream.AppendOne(new TicketClosed());

            await session.SaveChangesAsync();

            return Ok();
        }
    }
}
