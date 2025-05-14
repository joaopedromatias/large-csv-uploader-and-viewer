using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobProgressController : ControllerBase
{
    private readonly ILogger<JobProgressController> _logger;
    private readonly IJobProgressService _jobProgressService;
    public JobProgressController(ILogger<JobProgressController> logger, IJobProgressService jobProgressService)
    {
        _logger = logger;
        _jobProgressService = jobProgressService;
    }

    [HttpGet("Stream")]
    public async Task GetProgress([FromQuery] int jobId, CancellationToken cancellationToken)
    {
        Response.Headers.Append("Content-Type", "text/event-stream");
        
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var progress = _jobProgressService.GetProgress(jobId);

                await WriteEventAsync("message", progress.ToString(), cancellationToken);

                if (progress >= 100)
                {
                    await WriteEventAsync("done", "true", cancellationToken);
                    break;
                }

                await Task.Delay(200, cancellationToken);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            _logger.LogError($"Error while streaming job {jobId} progress: {ex.Message}");
            await WriteEventAsync("error", $"Server error: {ex.Message}", cancellationToken);
        }
    }

    private async Task WriteEventAsync(string eventName, string data, CancellationToken cancellationToken)
    {
        await Response.WriteAsync($"event: {eventName}\n", cancellationToken);
        await Response.WriteAsync($"data: {data}\n", cancellationToken);
        await Response.WriteAsync("\n", cancellationToken);
        await Response.Body.FlushAsync(cancellationToken);
    }
}
