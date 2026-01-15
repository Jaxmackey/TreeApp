using Microsoft.EntityFrameworkCore;
using TreeApp.Domain.Entities;
using TreeApp.Domain.Interfaces;
using TreeApp.Infrastructure.Data;

namespace TreeApp.Infrastructure.Repositories;

public class JournalRepository : IJournalRepository
{
    private readonly ApplicationDbContext _context;

    public JournalRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(JournalEntry entry, CancellationToken cancellationToken = default)
    {
        _context.JournalEntries.Add(entry);
    }

    public async Task<JournalEntry?> GetByEventIdAsync(long eventId, CancellationToken ct = default)
    {
        return await _context.JournalEntries
            .FirstOrDefaultAsync(j => j.EventId == eventId, ct);
    }

    public async Task<List<JournalEntry>> GetRangeAsync(
        int skip,
        int take,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        string? search = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.JournalEntries.AsQueryable();

        if (from.HasValue)
            query = query.Where(j => j.CreatedAt >= from.Value);
        if (to.HasValue)
            query = query.Where(j => j.CreatedAt <= to.Value);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(j => 
                (j.ExceptionMessage != null && j.ExceptionMessage.Contains(search)) ||
                (j.ExceptionType != null && j.ExceptionType.Contains(search)));

        return await query
            .OrderByDescending(j => j.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}