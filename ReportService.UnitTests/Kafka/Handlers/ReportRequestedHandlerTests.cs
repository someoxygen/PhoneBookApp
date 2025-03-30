using Moq;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Kafka.Handlers;
using Shared.Events;
using Xunit;
using Microsoft.Extensions.Logging;

namespace ReportService.UnitTests.Kafka.Handlers;

public class ReportRequestedHandlerTests
{
    private readonly Mock<IReportRepository> _repositoryMock = new();
    private readonly Mock<ILogger<ReportRequestedHandler>> _loggerMock = new();

    [Fact]
    public async Task HandleAsync_Should_Create_And_Update_Report()
    {
        var handler = new ReportRequestedHandler(_repositoryMock.Object, _loggerMock.Object);

        var evt = new ReportRequestedEvent
        {
            ReportId = Guid.NewGuid(),
            RequestedAt = DateTime.UtcNow
        };

        await handler.HandleAsync(evt);

        _repositoryMock.Verify(r => r.CreateAsync(It.Is<Report>(x => x.Id == evt.ReportId)), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Report>(x => x.Status == ReportStatus.Completed)), Times.Once);
    }
}
