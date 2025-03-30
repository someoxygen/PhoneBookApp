using MongoDB.Driver;
using Moq;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Context;
using ReportService.Infrastructure.Mongo.Repositories;
using Xunit;

namespace ReportService.UnitTests.Repositories;

public class ReportRepositoryTests
{
    private readonly Mock<IMongoCollection<Report>> _collectionMock;
    private readonly Mock<IMongoContext> _contextMock;
    private readonly ReportRepository _repository;

    public ReportRepositoryTests()
    {
        _collectionMock = new Mock<IMongoCollection<Report>>();
        _contextMock = new Mock<IMongoContext>();

        // IMongoContext.Reports özelliğini mockla
        _contextMock.Setup(c => c.Reports).Returns(_collectionMock.Object);

        _repository = new ReportRepository(_contextMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Call_InsertOneAsync()
    {
        var report = new Report();

        await _repository.CreateAsync(report);

        _collectionMock.Verify(x =>
            x.InsertOneAsync(report, null, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Call_ReplaceOneAsync()
    {
        var report = new Report { Id = Guid.NewGuid() };

        await _repository.UpdateAsync(report);

        _collectionMock.Verify(x =>
            x.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Report>>(),
                report,
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Report()
    {
        var reportId = Guid.NewGuid();
        var expected = new Report { Id = reportId };

        var cursorMock = new Mock<IAsyncCursor<Report>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(new List<Report> { expected });

        _collectionMock.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<Report>>(),
                It.IsAny<FindOptions<Report, Report>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _repository.GetByIdAsync(reportId);

        Assert.NotNull(result);
        Assert.Equal(reportId, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Reports()
    {
        var reports = new List<Report> { new Report() };

        var cursorMock = new Mock<IAsyncCursor<Report>>();
        cursorMock.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .ReturnsAsync(true)
                  .ReturnsAsync(false);
        cursorMock.SetupGet(c => c.Current).Returns(reports);

        _collectionMock.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<Report>>(),
                It.IsAny<FindOptions<Report, Report>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(cursorMock.Object);

        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Single(result);
    }
}
