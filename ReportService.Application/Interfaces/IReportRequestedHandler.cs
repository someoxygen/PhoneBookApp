using Shared.Events;

namespace ReportService.Application.Interfaces;

public interface IReportRequestedHandler
{
    Task HandleAsync(ReportRequestedEvent reportRequestedEvent);
}
