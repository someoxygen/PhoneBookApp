namespace Shared.Events;

public class ReportRequestedEvent
{
    public Guid ReportId { get; set; } = Guid.NewGuid();
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}
