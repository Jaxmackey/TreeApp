namespace TreeApp.Application.DTOs;

public class MJournalInfo
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public class MJournal : MJournalInfo
{
    public string Text { get; set; } = null!;
}

public class MRangeMJournalInfo
{
    public int Skip { get; set; }
    public int Count { get; set; }
    public List<MJournalInfo> Items { get; set; } = new();
}

public class VJournalFilter
{
    public DateTimeOffset? From { get; set; }
    public DateTimeOffset? To { get; set; }
    public string? Search { get; set; }
}