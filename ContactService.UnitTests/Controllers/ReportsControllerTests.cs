using ContactService.Infrastructure.Kafka;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.Events;

namespace ContactService.UnitTests.Controllers;

public class ReportsControllerTests
{
    private readonly Mock<IKafkaProducerService> _kafkaProducerMock;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        _kafkaProducerMock = new Mock<IKafkaProducerService>();
        _controller = new ReportsController(_kafkaProducerMock.Object);
    }

    [Fact]
    public async Task RequestReport_Should_Return_OkResult_With_ReportId()
    {
        // Arrange
        ReportRequestedEvent capturedEvent = null!;
        _kafkaProducerMock
            .Setup(x => x.ProduceReportRequestedEventAsync(It.IsAny<ReportRequestedEvent>()))
            .Callback<ReportRequestedEvent>(evt => capturedEvent = evt)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.RequestReport();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

        okResult.Value.Should().BeEquivalentTo(new
        {
            Message = "Rapor talebi alındı, işleniyor.",
            ReportId = capturedEvent.ReportId
        });

        _kafkaProducerMock.Verify(
            x => x.ProduceReportRequestedEventAsync(It.IsAny<ReportRequestedEvent>()),
            Times.Once);
    }
}
