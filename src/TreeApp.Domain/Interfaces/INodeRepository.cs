using TreeApp.Domain.Entities;

namespace TreeApp.Domain.Interfaces;

public interface INodeRepository
{
    Task<Node?> GetRootAsync(string treeName, CancellationToken cancellationToken = default);
    Task<Node?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<List<Node>> GetChildrenAsync(long parentId, CancellationToken cancellationToken = default);
    Task AddAsync(Node node, CancellationToken cancellationToken = default);
    Task DeleteSubtreeAsync(long nodeId, CancellationToken cancellationToken = default);
    Task<bool> HasChildrenAsync(long nodeId, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAmongSiblingsAsync(long? parentId, string name, string treeName, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}