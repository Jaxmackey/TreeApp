using MediatR;
using TreeApp.Domain.Exceptions;
using TreeApp.Domain.Interfaces;

namespace TreeApp.Application.Requests.Tree;

public record CreateNodeRequest(string TreeName, long? ParentNodeId, string NodeName) : IRequest<Unit>;

internal class CreateNodeHandler(INodeRepository nodeRepository) : IRequestHandler<CreateNodeRequest, Unit>
{
    public async Task<Unit> Handle(CreateNodeRequest request, CancellationToken ct)
    {
        if (request.ParentNodeId.HasValue)
        {
            var parent = await nodeRepository.GetByIdAsync(request.ParentNodeId.Value, ct);
            if (parent == null || parent.TreeName != request.TreeName)
                throw new SecureException("Parent node not found or belongs to another tree");
        }

        if (!await nodeRepository.IsNameUniqueAmongSiblingsAsync(
                request.ParentNodeId, request.NodeName, request.TreeName, ct))
        {
            throw new SecureException("Node name must be unique among siblings");
        }

        var node = new Domain.Entities.Node
        {
            Name = request.NodeName,
            TreeName = request.TreeName,
            ParentId = request.ParentNodeId
        };

        await nodeRepository.AddAsync(node, ct);
        await nodeRepository.SaveChangesAsync(ct);
        return Unit.Value;
    }
}