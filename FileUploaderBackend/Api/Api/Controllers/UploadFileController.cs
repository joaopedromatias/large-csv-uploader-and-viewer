using Application.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[RequestSizeLimit(50_000_000)]
[Route("api/[controller]")]
[ApiController]
public class UploadFileController : ControllerBase
{
    private readonly IFileService _fileService;

    public UploadFileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file.Length == 0)
            return BadRequest("Empty file");

        var jobId = await _fileService.PersistFile(file);
        
        BackgroundJob.Enqueue<IProcessDataService>(s => s.StartDataProcessing(jobId));

        return Ok(new { jobId });
    }
}
