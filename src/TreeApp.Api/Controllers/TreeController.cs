using MediatR;
using Microsoft.AspNetCore.Mvc;
using TreeApp.Application.DTOs;
using TreeApp.Application.Requests.Tree;

namespace TreeApp.Api.Controllers;

[ApiController]
public class TreeController(IMediator mediator) : ControllerBase
{
    [HttpPost("api.user.tree.get")]
    public async Task<ActionResult<MNode>> GetTree([FromQuery] string treeName, CancellationToken ct)
    {
        var result = await mediator.Send(new GetTreeRequest(treeName), ct);
        return Ok(result);
    }

    [HttpPost("api.user.tree.node.create")]
    public async Task<IActionResult> CreateNode(
        [FromQuery] string treeName,
        [FromQuery] long? parentNodeId,
        [FromQuery] string nodeName,
        CancellationToken ct)
    {
        await mediator.Send(new CreateNodeRequest(treeName, parentNodeId, nodeName), ct);
        return Ok();
    }

    [HttpPost("api.user.tree.node.delete")]
    public async Task<IActionResult> DeleteNode([FromQuery] long nodeId, CancellationToken ct)
    {
        await mediator.Send(new DeleteNodeRequest(nodeId), ct);
        return Ok();
    }

    [HttpPost("api.user.tree.node.rename")]
    public async Task<IActionResult> RenameNode(
        [FromQuery] long nodeId,
        [FromQuery] string newNodeName,
        CancellationToken ct)
    {
        await mediator.Send(new RenameNodeRequest(nodeId, newNodeName), ct);
        return Ok();
    }
}