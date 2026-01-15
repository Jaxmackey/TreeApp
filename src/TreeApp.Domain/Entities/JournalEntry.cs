namespace TreeApp.Domain.Entities;

public class JournalEntry
{
    public long Id { get; set; }
    public long EventId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? QueryString { get; set; }
    public string? RequestBody { get; set; }
    public string? StackTrace { get; set; }
    public string? ExceptionType { get; set; }
    public string? ExceptionMessage { get; set; }
}