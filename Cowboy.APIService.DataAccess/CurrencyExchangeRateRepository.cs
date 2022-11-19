using Cowboy.APIService.Contracts;
using Cowboy.APIService.DataAccess.Helpers;
using Cowboy.APIService.Models;
using Cowboy.APIService.Models.ViewModels;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowboy.APIService.DataAccess
{
    public class CurrencyExchangeRateRepository : ICurrencyExchangeRateRepository
    {
        private readonly IDbConnection _connection;

        public CurrencyExchangeRateRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public CurrencyExchangeRateRepository()
        {
        }

        public async Task<CurrencyExchangeRateModel?> GetCurrencyExchangeRate(string sourceCurrencyCode, string targetCurrencyCode, long amount, DateTime? date)
        {
            // Call API here
            string baseURI = "https://api.apilayer.com";
            string requestParam = date == null ? String.Format(Constants.ExternalApiLatestRateRoute, targetCurrencyCode, sourceCurrencyCode)
                : String.Format(Constants.ExternalApiGivenDateRateRoute, date.Value.ToString("yyyy-MM-dd"), targetCurrencyCode, sourceCurrencyCode);

            string responseBody = HttpClientHelper.DoHttpRequest("GET", baseURI, requestParam);

            var currencyExchangeRateModel = JsonConvert.DeserializeObject<CurrencyExchangeRateModel>(responseBody);
            if (currencyExchangeRateModel == null)
                return null;

            var keys = currencyExchangeRateModel.ExchangeRates.Keys;
            foreach (var key in keys)
            {
                currencyExchangeRateModel.ExchangeRates[key] = Convert.ToString(Convert.ToDecimal(currencyExchangeRateModel.ExchangeRates[key]) * amount);
            }

            return currencyExchangeRateModel;
        }

        public CurrencyExchangeRateOnPeriodModel? GetCurrencyExchangeRateByPeriod(string sourceCurrencyCode, DateTime fromDate, DateTime toDate)
        {
            var sql = $"SELECT CER.*, SC.Code as SourceCurrencyCode, TC.Code as TargetCurrencyCode  " +
                        $"FROM CurrencyExchangeRates CER " +
                        $"INNER JOIN Currency SC ON SC.Id = CER.SourceCurrencyId AND SC.Code = '{sourceCurrencyCode}' " +
                        $"INNER JOIN Currency TC  ON TC.Id = CER.TargetCurrencyId " +
                        $"AND CER.RecordedOn BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}'";

            var currencyExchangeRate = _connection.Query<CurrencyExchangeRate>(sql);

            var result = currencyExchangeRate.GroupBy(x => new { x.SourceCurrencyId, x.SourceCurrencyCode })
                .Select(x => new CurrencyExchangeRateOnPeriodModel
                {
                    SourceCurrencyCode = x.Key.SourceCurrencyCode,
                    StartDate = fromDate,
                    EndDate = toDate,
                    ExchangeRates = x.ToList().GroupBy(g => new { g.RecordedOn })
                                              .ToDictionary(v => v.Key.RecordedOn, 
                                                            v => v.ToDictionary(d => d.TargetCurrencyCode, d => d.TargetCurrencyExchangeRate))
                }).FirstOrDefault();

            return result;
        }
    }
}
