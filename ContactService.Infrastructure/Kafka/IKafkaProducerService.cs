using Shared.Events;

namespace ContactService.Infrastructure.Kafka;

public interface IKafkaProducerService
{
    Task ProduceReportRequestedEventAsync(ReportRequestedEvent @event);
}
