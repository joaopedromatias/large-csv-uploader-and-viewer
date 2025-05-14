using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services;

public class JobProgressService : IJobProgressService
{
    private const int TTL_SECONDS = 10;
    private static DateTimeOffset Ttl => DateTimeOffset.UtcNow.AddSeconds(TTL_SECONDS);
    private readonly IMemoryCache _memoryCache;

    public JobProgressService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public int GetProgress(int jobId)
    {
        return _memoryCache.Get<int>(jobId);
    }

    public void SetProgress(int jobId, int progress)
    {
        _memoryCache.Set(jobId, progress, Ttl);
    }
}
