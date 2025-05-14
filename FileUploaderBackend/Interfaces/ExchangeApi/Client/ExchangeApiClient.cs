using ExchangeApi.DTOs;
using Newtonsoft.Json;
using Application.Interfaces;
using Application.DTOs;

namespace ExchangeApi.Client;

public class ExchangeApiClient : IExchangeApiClient
{
    private readonly HttpClient _httpClient;

    public ExchangeApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<ExchangeDto>> GetExchangeData()
    {
        var response = await _httpClient.GetAsync("currencies/usd.min.json");
        
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var exchangeData = JsonConvert.DeserializeObject<ExchangeApiDto>(content)!.Usd;

            var exchangesDataDto = new List<ExchangeDto>
            {
                new ExchangeDto { CurrencyCode = "Ars", RateToUsd = exchangeData.Ars },
                new ExchangeDto { CurrencyCode = "Brl", RateToUsd = exchangeData.Brl },
                new ExchangeDto { CurrencyCode = "Jpy", RateToUsd = exchangeData.Jpy },
                new ExchangeDto { CurrencyCode = "Eur", RateToUsd = exchangeData.Eur },
                new ExchangeDto { CurrencyCode = "Gbp", RateToUsd = exchangeData.Gbp }
            };

            return exchangesDataDto;
        }
        else
        {
            throw new Exception($"Error while fetching Exchange Api: {response.Content}");
        }
    }
}
