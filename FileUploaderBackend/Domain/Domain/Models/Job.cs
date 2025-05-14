using Domain.Enums;

namespace Domain.Models;

public class Job
{
    public int Id { get; private set; }
    public JobStatus Status { get; private set; } = JobStatus.Pending;

    public Job () { }
    
    internal Job(int id, JobStatus status)
    {
        Id = id;
        Status = status;
    }

    public void SetStatus(JobStatus status)
    {
        Status = status;
    }
}