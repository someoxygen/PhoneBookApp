using ContactService.Infrastructure.Kafka;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Shared.Events;

namespace ContactService.UnitTests.Kafka;

public class KafkaProducerServiceTests
{
    private readonly KafkaProducerService _producerService;

    public KafkaProducerServiceTests()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["Kafka:BootstrapServers"]).Returns("localhost:9092");

        _producerService = new KafkaProducerService(configMock.Object);
    }

    [Fact]
    public async Task ProduceReportRequestedEvent_Should_Not_Throw_Exception()
    {
        var @event = new ReportRequestedEvent();

        Func<Task> act = async () => await _producerService.ProduceReportRequestedEventAsync(@event);

        await act.Should().NotThrowAsync();
    }
}
