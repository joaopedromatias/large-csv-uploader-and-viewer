namespace Application.Interfaces;

public interface IJobProgressService
{
    int GetProgress(int jobId);
    void SetProgress(int jobId, int progress);
}
