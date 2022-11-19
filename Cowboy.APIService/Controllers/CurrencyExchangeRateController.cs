using Cowboy.APIService.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cowboy.APIService.Controllers
{
    [Route("api/rates")]
    public class CurrencyExchangeRateController : Controller
    {
        private readonly ICurrencyExchangeRateRepository currencyExchangeRateRepository;

        public CurrencyExchangeRateController(ICurrencyExchangeRateRepository currencyExchangeRateRepository)
        {
            this.currencyExchangeRateRepository = currencyExchangeRateRepository;
        }

        // GET api/rates/{sourceCurrency}/{targetCurrency}/{amount}
        [HttpGet]
        [Route("{sourceCurrency}/{targetCurrency}/{amount}")]
        public async Task<CurrencyExchangeRateModel> GetCurrencyExchangeRate(string sourceCurrency, string targetCurrency, long amount, DateTime? date)
        {
            var exchangeRates = await currencyExchangeRateRepository.GetCurrencyExchangeRate(sourceCurrency, targetCurrency, amount, date);

            return exchangeRates;
        }

        // GET api/rates/{sourceCurrency}/{targetCurrency}/{from}/{to}
        [HttpGet]
        [Route("period/{sourceCurrency}/{fromDate}/{toDate}")]
        public CurrencyExchangeRateOnPeriodModel GetCurrencyExchangeRate(string sourceCurrency, DateTime fromDate, DateTime toDate)
        {
            var result = currencyExchangeRateRepository.GetCurrencyExchangeRateByPeriod(sourceCurrency, fromDate, toDate);

            return result;
        }
    }
}
