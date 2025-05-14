using System.Globalization;
using System.Threading.Channels;
using Application.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Application.Constants;

namespace Application.Services;

public class FileService : IFileService
{
    public const string BASE_FILE_PATH = "./tmp/uploads";

    public async Task<int> PersistFile(IFormFile file)
    {
        var jobId = Random.Shared.Next(1, int.MaxValue);

        var filePath = Path.Combine(BASE_FILE_PATH, $"{jobId}.csv");

        Directory.CreateDirectory(BASE_FILE_PATH);

        using (var stream = new FileStream(filePath, FileMode.CreateNew))
        {
            await file.CopyToAsync(stream);
        }
        
        return jobId;
    }

    public void DeleteFile(int jobId)
    {
        var filePath = Path.Combine(BASE_FILE_PATH, $"{jobId}.csv");
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            throw new Exception($"File not found: {filePath}");
        }
    }

    public async Task ProcessCsvFile<T>(int jobId, Func<IEnumerable<T>, int, Task> processFn)
    {
        var filePath = Path.Combine(BASE_FILE_PATH, $"{jobId}.csv");
        
        if (!File.Exists(filePath))
            throw new Exception($"File not found: {filePath}");

        var channel = Channel.CreateBounded<(T record, int progress)>(
            new BoundedChannelOptions(2_000)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleWriter = true,
                SingleReader = false,
            }
        ); 
        
        var consumers = new Task[Environment.ProcessorCount];
        for (var i = 0; i < consumers.Count(); i++)
        {
            consumers[i] = Task.Run(async () => 
            {
                var records = new List<T>();
                int currentProgress = 0;

                await foreach (var (record, progress) in channel.Reader.ReadAllAsync())
                { 
                    records.Add(record);
                    currentProgress = progress;

                    if (records.Count >= AppConstants.BATCH_SIZE)
                    {     
                        await processFn(records, currentProgress);
                        records.Clear();
                    }      
                }
                if (records.Count > 0)
                {
                    await processFn(records, currentProgress);
                    records.Clear();
                }
            });
        }

        using var str = new StreamReader(filePath);
        using var csv = new CsvReader(str, CsvReadConfiguration());

        await foreach (var record in csv.GetRecordsAsync<T>())
        {
            var progress = (int)Math.Min(str.BaseStream.Position * 100 / str.BaseStream.Length, 99);
            await channel.Writer.WriteAsync((record, progress));
        }

        channel.Writer.Complete();
        await Task.WhenAll(consumers);
    }

    private static CsvConfiguration CsvReadConfiguration() 
    {
        return new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            IgnoreBlankLines = true,
            HeaderValidated = null,
            MissingFieldFound = null,
            Delimiter = ";"
        };
    }
}
