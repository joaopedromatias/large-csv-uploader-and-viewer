using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileService
{
    Task<int> PersistFile(IFormFile file);
    void DeleteFile(int jobId);
    Task ProcessCsvFile<T>(int jobId, Func<IEnumerable<T>, int, Task> processFn);
}
