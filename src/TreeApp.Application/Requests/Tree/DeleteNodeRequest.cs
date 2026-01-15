using MediatR;
using TreeApp.Domain.Exceptions;
using TreeApp.Domain.Interfaces;

namespace TreeApp.Application.Requests.Tree;

public record DeleteNodeRequest(long NodeId) : IRequest<Unit>;

internal class DeleteNodeHandler(INodeRepository nodeRepository) : IRequestHandler<DeleteNodeRequest, Unit>
{
    public async Task<Unit> Handle(DeleteNodeRequest request, CancellationToken ct)
    {
        var node = await nodeRepository.GetByIdAsync(request.NodeId, ct);
        if (node == null)
            throw new SecureException("Node not found");
        await nodeRepository.DeleteSubtreeAsync(request.NodeId, ct);
        await nodeRepository.SaveChangesAsync(ct);
        return Unit.Value;
    }
}