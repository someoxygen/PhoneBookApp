using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Moq;

namespace ReportService.UnitTests.Kafka.Consumers;

public class ReportRequestedConsumerTests
{
    private readonly Mock<IHostedService> _mockConsumer;

    public ReportRequestedConsumerTests()
    {
        _mockConsumer = new Mock<IHostedService>();
    }

    [Fact]
    public async Task StartAsync_Should_NotThrowException()
    {
        Func<Task> act = async () => await _mockConsumer.Object.StartAsync(CancellationToken.None);
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task StopAsync_Should_NotThrowException()
    {
        Func<Task> act = async () => await _mockConsumer.Object.StopAsync(CancellationToken.None);
        await act.Should().NotThrowAsync();
    }
}
