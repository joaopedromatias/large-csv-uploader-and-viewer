using Domain.Enums;
using Domain.Models;

namespace Domain.Factories;

public static class JobFactory
{
    public static Job CreateJob(int jobId, JobStatus status) 
    {
        var job = new Job(jobId, status);
        return job;
    }
}
