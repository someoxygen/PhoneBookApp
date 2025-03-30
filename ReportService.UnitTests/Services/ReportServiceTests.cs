using FluentAssertions;
using Moq;
using ReportService.Application.Interfaces;
using ReportService.Application.Services;
using ReportService.Domain.Entities;

namespace ReportService.UnitTests.Services;

public class ReportServiceTests
{
    private readonly Mock<IReportRepository> _repositoryMock;
    private readonly ReportService.Application.Services.ReportingService _service;

    public ReportServiceTests()
    {
        _repositoryMock = new Mock<IReportRepository>();
        _service = new ReportService.Application.Services.ReportingService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllReportsAsync_Should_Return_All_Reports()
    {
        // Arrange
        var reports = new List<Report>
        {
            new() { Id = Guid.NewGuid(), Status = ReportStatus.Completed },
            new() { Id = Guid.NewGuid(), Status = ReportStatus.Preparing }
        };

        _repositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(reports);

        // Act
        var result = await _service.GetAllReportsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(reports);
    }

    [Fact]
    public async Task GetReportByIdAsync_Should_Return_Report_When_Exists()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var report = new Report { Id = reportId, Status = ReportStatus.Completed };

        _repositoryMock.Setup(repo => repo.GetByIdAsync(reportId)).ReturnsAsync(report);

        // Act
        var result = await _service.GetReportByIdAsync(reportId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(report);
    }

    [Fact]
    public async Task GetReportByIdAsync_Should_Return_Null_When_Not_Exists()
    {
        // Arrange
        var reportId = Guid.NewGuid();

        _repositoryMock.Setup(repo => repo.GetByIdAsync(reportId)).ReturnsAsync((Report?)null);

        // Act
        var result = await _service.GetReportByIdAsync(reportId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateReportAsync_Should_Call_Repository_CreateAsync()
    {
        // Arrange
        var report = new Report { Id = Guid.NewGuid(), Status = ReportStatus.Preparing };

        // Act
        await _service.CreateReportAsync(report);

        // Assert
        _repositoryMock.Verify(repo => repo.CreateAsync(report), Times.Once);
    }

    [Fact]
    public async Task UpdateReportAsync_Should_Call_Repository_UpdateAsync()
    {
        // Arrange
        var report = new Report { Id = Guid.NewGuid(), Status = ReportStatus.Completed };

        // Act
        await _service.UpdateReportAsync(report);

        // Assert
        _repositoryMock.Verify(repo => repo.UpdateAsync(report), Times.Once);
    }
    [Fact]
    public async Task GetReportByIdAsync_Should_Return_Report()
    {
        var reportId = Guid.NewGuid();
        var expectedReport = new Report { Id = reportId };

        _repositoryMock.Setup(x => x.GetByIdAsync(reportId))
                       .ReturnsAsync(expectedReport);

        var result = await _service.GetReportByIdAsync(reportId);

        result.Should().BeEquivalentTo(expectedReport);
    }

    [Fact]
    public async Task GetReportByIdAsync_Should_Return_Null_If_Report_NotFound()
    {
        var reportId = Guid.NewGuid();

        _repositoryMock.Setup(x => x.GetByIdAsync(reportId))
                       .ReturnsAsync((Report?)null);

        var result = await _service.GetReportByIdAsync(reportId);

        result.Should().BeNull();
    }

}
