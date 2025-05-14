using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundJobs;

public static class BackgroundJobs
{
    public static void Configure(IServiceCollection services, string connString)
    {
        services.AddHangfire((provider, configuration) => configuration 
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseColouredConsoleLogProvider()
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseFilter(new AutomaticRetryAttribute {Attempts = 5})
            .UseSqlServerStorage(connString)
            .UseActivator(new HangfireActivator(provider))
        );
    }
}

public class HangfireActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    public HangfireActivator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override object ActivateJob(Type type)
    {
        return _serviceProvider.GetRequiredService(type);
    }
}  