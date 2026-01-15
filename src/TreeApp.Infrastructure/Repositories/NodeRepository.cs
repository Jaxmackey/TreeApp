using Microsoft.EntityFrameworkCore;
using TreeApp.Domain.Entities;
using TreeApp.Domain.Interfaces;
using TreeApp.Infrastructure.Data;

namespace TreeApp.Infrastructure.Repositories;

public class NodeRepository : INodeRepository
{
    private readonly ApplicationDbContext _context;

    public NodeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Node?> GetRootAsync(string treeName, CancellationToken cancellationToken = default)
        => await _context.Nodes
            .FirstOrDefaultAsync(n => n.TreeName == treeName && n.ParentId == null, cancellationToken);

    public async Task<Node?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await _context.Nodes.FindAsync(new object[] { id }, cancellationToken);

    public async Task<List<Node>> GetChildrenAsync(long parentId, CancellationToken cancellationToken = default)
        => await _context.Nodes
            .Where(n => n.ParentId == parentId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Node node, CancellationToken cancellationToken = default)
    {
        _context.Nodes.Add(node);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSubtreeAsync(long nodeId, CancellationToken cancellationToken = default)
    {
        var node = await _context.Nodes.FindAsync(new object[] { nodeId }, cancellationToken);
        if (node != null)
            _context.Nodes.Remove(node);
    }

    public async Task<bool> HasChildrenAsync(long nodeId, CancellationToken cancellationToken = default)
        => await _context.Nodes.AnyAsync(n => n.ParentId == nodeId, cancellationToken);

    public async Task<bool> IsNameUniqueAmongSiblingsAsync(long? parentId, string name, string treeName, CancellationToken cancellationToken = default)
        => !await _context.Nodes
            .AnyAsync(n => n.ParentId == parentId && n.Name == name && n.TreeName == treeName, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}