using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReportService.Domain.Entities;

public class Report
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    public ReportStatus Status { get; set; } = ReportStatus.Preparing;

    public List<ReportDetail> Details { get; set; } = new();
}

public enum ReportStatus
{
    Preparing,
    Completed
}
