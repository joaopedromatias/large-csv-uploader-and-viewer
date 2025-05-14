namespace Application.Interfaces;

public interface IProcessDataService
{
    Task StartDataProcessing(int jobId);
}
