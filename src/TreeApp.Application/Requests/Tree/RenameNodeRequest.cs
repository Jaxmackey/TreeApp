using MediatR;
using TreeApp.Domain.Exceptions;
using TreeApp.Domain.Interfaces;

namespace TreeApp.Application.Requests.Tree;

public record RenameNodeRequest(long NodeId, string NewNodeName) : IRequest<Unit>;

public class RenameNodeHandler(INodeRepository nodeRepository) : IRequestHandler<RenameNodeRequest, Unit>
{
    public async Task<Unit> Handle(RenameNodeRequest request, CancellationToken ct)
    {
        var node = await nodeRepository.GetByIdAsync(request.NodeId, ct);
        if (node == null)
            throw new SecureException("Node not found");
        
        if (!await nodeRepository.IsNameUniqueAmongSiblingsAsync(
                node.ParentId, request.NewNodeName, node.TreeName, ct))
        {
            throw new SecureException("New node name must be unique among siblings");
        }

        node.Name = request.NewNodeName;
        await nodeRepository.SaveChangesAsync(ct);
        return Unit.Value;
    }
}