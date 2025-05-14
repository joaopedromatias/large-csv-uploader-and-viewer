using Application.Interfaces;
using Application.Services;
using Data.Context;
using Data.Repositories;
using Data.UnitOfWork;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using ExchangeApi.Client;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;

namespace IoC;

public static class IoC
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration) 
    { 
        services.AddSingleton<IJobProgressService, JobProgressService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<IProcessDataService, ProcessDataService>();
        services.AddScoped<IProductService, ProductService>();
    }

    public static void AddClients(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddHttpClient<IExchangeApiClient, ExchangeApiClient>(client => 
        { 
            var baseUrl = configuration["ExchangeApi:BaseUrl"];

            client.BaseAddress = new Uri(baseUrl!);
            client.Timeout = TimeSpan.FromSeconds(5);
        })
        .AddPolicyHandler(GetExponentialBackoffRetryPolicy());
    }

    public static void AddLogging(this IServiceCollection services, WebApplicationBuilder builder) 
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    public static void AddData(this IServiceCollection services, IConfiguration configuration) 
    { 
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<FileUploaderContext>(options => 
        {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IExchangeRepository, ExchangeRepository>();
    }

    public static void AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration) 
    { 
        BackgroundJobs.BackgroundJobs.Configure(services, configuration.GetConnectionString("DefaultConnection")!);
        services.AddHangfireServer();
    }    

    private static IAsyncPolicy<HttpResponseMessage> GetExponentialBackoffRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() 
            .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
