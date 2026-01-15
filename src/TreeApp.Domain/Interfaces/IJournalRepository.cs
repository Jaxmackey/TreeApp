using TreeApp.Domain.Entities;

namespace TreeApp.Domain.Interfaces;

public interface IJournalRepository
{
    Task AddAsync(JournalEntry entry, CancellationToken cancellationToken = default);
    Task<JournalEntry?> GetByEventIdAsync(long eventId, CancellationToken ct = default);
    Task<List<JournalEntry>> GetRangeAsync(
        int skip,
        int take,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null,
        string? search = null,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}