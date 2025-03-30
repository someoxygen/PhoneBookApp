using System.Diagnostics.CodeAnalysis;

namespace ReportService.Infrastructure.Mongo.Settings;

[ExcludeFromCodeCoverage]
public class MongoSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}
