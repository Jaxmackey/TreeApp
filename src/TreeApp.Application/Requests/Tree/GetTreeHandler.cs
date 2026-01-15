using MediatR;
using TreeApp.Application.DTOs;
using TreeApp.Domain.Interfaces;

namespace TreeApp.Application.Requests.Tree;

public class GetTreeHandler(INodeRepository nodeRepository) : IRequestHandler<GetTreeRequest, MNode>
{
    public async Task<MNode> Handle(GetTreeRequest request, CancellationToken cancellationToken)
    {
        var root = await nodeRepository.GetRootAsync(request.TreeName, cancellationToken);
        if (root == null)
        {
            root = new Domain.Entities.Node
            {
                Name = request.TreeName,
                TreeName = request.TreeName,
                ParentId = null
            };
            await nodeRepository.AddAsync(root, cancellationToken);
            await nodeRepository.SaveChangesAsync(cancellationToken);
        }

        return await BuildTreeAsync(root.Id, cancellationToken);
    }

    private async Task<MNode> BuildTreeAsync(long nodeId, CancellationToken ct)
    {
        var node = await nodeRepository.GetByIdAsync(nodeId, ct) 
                   ?? throw new InvalidOperationException("Node not found");
        var children = await nodeRepository.GetChildrenAsync(nodeId, ct);

        return new MNode
        {
            Id = node.Id,
            Name = node.Name,
            Children = (await Task.WhenAll(children.Select(c => BuildTreeAsync(c.Id, ct)))).ToList()
        };
    }
}