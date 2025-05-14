using Application.DTOs;

namespace Application.Interfaces;

public interface IExchangeApiClient
{
    Task<List<ExchangeDto>> GetExchangeData();
}
