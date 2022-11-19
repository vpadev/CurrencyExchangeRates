using Cowboy.APIService.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowboy.APIService.Contracts
{
    public interface ICurrencyExchangeRateRepository
    {
        Task<CurrencyExchangeRateModel?> GetCurrencyExchangeRate(string sourceCurrencyCode, string targetCurrencyCode, long amount, DateTime? date);
        CurrencyExchangeRateOnPeriodModel? GetCurrencyExchangeRateByPeriod(string sourceCurrencyCode, DateTime fromDate, DateTime toDate);
        //CurrencyExchangeRateOnPeriodModel? SaveCurrencyExchangeRates(string sourceCurrencyCode, string targetCurrencyCode, long amount, DateTime? date);
        //Task<List<CurrencyExchangeRate>> GetCurrencyExchangeRatesByPeriod(DateTime fromDate, DateTime toDate);
    }
}
