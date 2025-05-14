using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Domain.Factories;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ProcessDataService : IProcessDataService
{  
    private readonly IJobProgressService _jobProgressService;
    private readonly IFileService _fileService;
    private readonly IExchangeApiClient _exchangeApiClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private ILogger<ProcessDataService> _logger;
    private IProductRepository _productRepository;
    private IExchangeRepository _exchangeRepository;
    private IJobRepository _jobRepository;
    private IUnitOfWork _unitOfWork;

    public ProcessDataService(
        IJobProgressService jobProgressService,
        IFileService fileService, 
        IExchangeApiClient exchangeApiClient,
        IServiceScopeFactory serviceScopeFactory)
    {        
        _jobProgressService = jobProgressService;
        _fileService = fileService;
        _exchangeApiClient = exchangeApiClient;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartDataProcessing(int jobId)
    {
        InitializeServices();                

        _logger.LogInformation($"Starting process of job {jobId}");
        _jobProgressService.SetProgress(jobId, 0);

        Job? job = null;

        try 
        {            
            job = await CreateJob(jobId);
            _logger.LogInformation($"JobId {jobId} | Job created");

            var exchangesDto = await FetchExchanges();
            _logger.LogInformation($"JobId {jobId} | Exchanges fetched");

            await CreateExchanges(exchangesDto, jobId);
            _logger.LogInformation($"JobId {jobId} | Exchanges saved");

            await _unitOfWork.SaveAsync();

            await ProcessProducts(jobId);
            _logger.LogInformation($"JobId {jobId} | Products saved");

            _fileService.DeleteFile(jobId);    

            job.SetStatus(JobStatus.Done);
            _logger.LogInformation($"JobId {jobId} | Jod finished sucessfully");
        }
        catch (Exception ex)
        {
            if (job != null)
                job.SetStatus(JobStatus.Error);

            await Task.WhenAll
            (
                _exchangeRepository.DeleteAllFromJob(jobId),
                _productRepository.DeleteAllFromJob(jobId)
            );
            _logger.LogError($"JobId {jobId} | Error while processing job {ex.Message}");
            throw;
        }    
        finally
        {
            await _unitOfWork.SaveAsync();
            _jobProgressService.SetProgress(jobId, 100);
        }
    }

    # region Initiliaze

    private void InitializeServices()
    {
        var scope = _serviceScopeFactory.CreateAsyncScope();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<ProcessDataService>();
        _productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        _exchangeRepository = scope.ServiceProvider.GetRequiredService<IExchangeRepository>();
        _jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }

    # endregion 

    #region Job
    
    private async Task<Job> CreateJob(int jobId)
    {
        var job = JobFactory.CreateJob(jobId, JobStatus.Processing);
        await _jobRepository.Create(job);
        return job;
    }

    #endregion

    #region Exchanges

    private async Task<List<ExchangeDto>> FetchExchanges()
    {
        var exchangesDto = await _exchangeApiClient.GetExchangeData();   
        return exchangesDto;
    }

    private async Task CreateExchanges(List<ExchangeDto> exchangesDto, int jobId)
    {
        var exchanges = new List<Exchange>();

        foreach (var exchange in exchangesDto)
        {
            exchanges.Add(ExchangeFactory.CreateExchange(exchange.CurrencyCode, exchange.RateToUsd, jobId));
        }

        await _exchangeRepository.CreateBatch(exchanges);
    }

    #endregion

    #region Products

    private async Task ProcessProducts(int jobId)
    {
        await _fileService.ProcessCsvFile<ProductFileDto>(jobId, async (productsDto, progress) => 
        {
            var products = new List<Product>();
            _jobProgressService.SetProgress(jobId, progress);

            foreach (var productDto in productsDto)
            {
                 if (productDto.Name == null || productDto.Price == null || productDto.Expiration == null) 
                {
                    _logger.LogWarning($"invalid product data, skipping creation | {productDto}");
                }
                else 
                {
                    products.Add(ProductFactory.CreateProduct(productDto.Name, productDto.Price.Value, productDto.Expiration.Value, jobId));
                }
            }

            if (products.Count > 0)
            {   
                await _productRepository.CreateBatch(products);
            }
        });
    }

    #endregion
}