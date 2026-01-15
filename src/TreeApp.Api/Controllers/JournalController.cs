using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreeApp.Application.DTOs;
using TreeApp.Application.Requests.Journal;

namespace TreeApp.Api.Controllers;

[ApiController]
public class JournalController : ControllerBase
{
    private readonly IMediator _mediator;

    public JournalController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("api.user.journal.getRange")]
    public async Task<ActionResult<MRangeMJournalInfo>> GetRange(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromBody] VJournalFilter? filter = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetJournalRangeRequest(skip, take, filter), ct);
        return Ok(result);
    }

    [HttpPost("api.user.journal.getSingle")]
    public async Task<ActionResult<MJournal?>> GetSingle([FromQuery] long eventId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetJournalEntryRequest(eventId), ct);
        return result == null ? NotFound() : Ok(result);
    }
}