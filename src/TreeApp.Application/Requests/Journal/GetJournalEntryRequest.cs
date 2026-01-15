using MediatR;
using TreeApp.Application.DTOs;
using TreeApp.Domain.Interfaces;

namespace TreeApp.Application.Requests.Journal;

public record GetJournalEntryRequest(long EventId) : IRequest<MJournal?>;

internal class GetJournalEntryHandler(IJournalRepository journalRepository)
    : IRequestHandler<GetJournalEntryRequest, MJournal?>
{
    public async Task<MJournal?> Handle(GetJournalEntryRequest request, CancellationToken ct)
    {
        var entry = await journalRepository.GetByEventIdAsync(request.EventId, ct);
        return entry == null ? null : new MJournal
        {
            Id = entry.Id,
            EventId = entry.EventId,
            CreatedAt = entry.CreatedAt,
            Text = $"[{entry.ExceptionType}] {entry.ExceptionMessage}\n{entry.StackTrace}"
        };
    }
}