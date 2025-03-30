using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReportService.API.Controllers;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;

namespace ReportService.UnitTests.Controllers;

public class ReportingControllerTests
{
    private readonly Mock<IReportingService> _reportingServiceMock;
    private readonly ReportingController _controller;

    public ReportingControllerTests()
    {
        _reportingServiceMock = new Mock<IReportingService>();
        _controller = new ReportingController(_reportingServiceMock.Object);
    }

    [Fact]
    public async Task GetAllReports_Should_Return_OkResult()
    {
        // Arrange
        var reports = new List<Report> { new Report() };
        _reportingServiceMock.Setup(s => s.GetAllReportsAsync()).ReturnsAsync(reports);

        // Act
        var result = await _controller.GetAllReports();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(reports);
    }

    [Fact]
    public async Task GetReportById_Should_Return_OkResult_When_ReportExists()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var report = new Report { Id = reportId };
        _reportingServiceMock.Setup(s => s.GetReportByIdAsync(reportId)).ReturnsAsync(report);

        // Act
        var result = await _controller.GetReportById(reportId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(report);
    }

    [Fact]
    public async Task GetReportById_Should_Return_NotFound_When_ReportNull()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        _reportingServiceMock.Setup(s => s.GetReportByIdAsync(reportId)).ReturnsAsync((Report?)null);

        // Act
        var result = await _controller.GetReportById(reportId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }
    [Fact]
    public async Task GetReportById_Should_Return_Ok_When_ReportExists()
    {
        // Arrange
        var reportId = Guid.NewGuid();
        var report = new Report { Id = reportId };

        _reportingServiceMock.Setup(x => x.GetReportByIdAsync(reportId)).ReturnsAsync(report);

        // Act
        var result = await _controller.GetReportById(reportId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(report);
    }

    [Fact]
    public async Task GetReportById_Should_Return_NotFound_When_ReportNotExists()
    {
        // Arrange
        var reportId = Guid.NewGuid();

        _reportingServiceMock.Setup(x => x.GetReportByIdAsync(reportId)).ReturnsAsync((Report?)null);

        // Act
        var result = await _controller.GetReportById(reportId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

}
