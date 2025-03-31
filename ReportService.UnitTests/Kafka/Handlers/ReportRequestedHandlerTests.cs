using Moq;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Kafka.Handlers;
using Shared.Events;
using Xunit;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace ReportService.UnitTests.Kafka.Handlers;

public class ReportRequestedHandlerTests
{
    private readonly Mock<IReportRepository> _repositoryMock = new();
    private readonly Mock<ILogger<ReportRequestedHandler>> _loggerMock = new();

    [Fact]
    public async Task HandleAsync_Should_Create_And_Update_Report()
    {
        // Arrange
        var httpClient = new HttpClient(new FakeHttpMessageHandler())
        {
            BaseAddress = new Uri("http://localhost:5211")
        };

        var handler = new ReportRequestedHandler(
            _repositoryMock.Object,
            httpClient,
            _loggerMock.Object);

        var evt = new ReportRequestedEvent
        {
            ReportId = Guid.NewGuid(),
            RequestedAt = DateTime.UtcNow
        };

        // Act
        await handler.HandleAsync(evt);

        // Assert
        _repositoryMock.Verify(r => r.CreateAsync(It.Is<Report>(x => x.Id == evt.ReportId)), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Report>(x => x.Status == ReportStatus.Completed)), Times.Once);
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private static readonly string FakePersonId = "b15d3b4a-5ed3-4fd1-bfc9-a1f253dc5d8a";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string json;

            if (request.RequestUri!.AbsolutePath.Contains("/api/person/all"))
            {
                json = $$"""
            [
                {
                    "id": "{{FakePersonId}}",
                    "name": "Test",
                    "surname": "Kullanici",
                    "company": "Fake"
                }
            ]
            """;
            }
            else if (request.RequestUri.AbsolutePath.Contains("/api/contactinformation/person"))
            {
                json = $$"""
            [
                {
                    "id": "{{Guid.NewGuid()}}",
                    "personId": "{{FakePersonId}}",
                    "type": 2,
                    "content": "İstanbul"
                },
                {
                    "id": "{{Guid.NewGuid()}}",
                    "personId": "{{FakePersonId}}",
                    "type": 0,
                    "content": "555-1234"
                }
            ]
            """;
            }
            else
            {
                json = "[]";
            }

            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }
    }


}
