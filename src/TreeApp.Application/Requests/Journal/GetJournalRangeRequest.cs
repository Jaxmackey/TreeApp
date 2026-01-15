using MediatR;
using TreeApp.Application.DTOs;
using TreeApp.Domain.Interfaces;

namespace TreeApp.Application.Requests.Journal;

public record GetJournalRangeRequest(int Skip, int Take, VJournalFilter? Filter) : IRequest<MRangeMJournalInfo>;

public class GetJournalRangeHandler : IRequestHandler<GetJournalRangeRequest, MRangeMJournalInfo>
{
    private readonly IJournalRepository _journalRepository;

    public GetJournalRangeHandler(IJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    public async Task<MRangeMJournalInfo> Handle(GetJournalRangeRequest request, CancellationToken ct)
    {
        var entries = await _journalRepository.GetRangeAsync(
            request.Skip,
            request.Take,
            request.Filter?.From,
            request.Filter?.To,
            request.Filter?.Search,
            ct);

        var items = entries.Select(e => new MJournalInfo
        {
            Id = e.Id,
            EventId = e.EventId,
            CreatedAt = e.CreatedAt
        }).ToList();

        return new MRangeMJournalInfo
        {
            Skip = request.Skip,
            Count = items.Count,
            Items = items
        };
    }
}