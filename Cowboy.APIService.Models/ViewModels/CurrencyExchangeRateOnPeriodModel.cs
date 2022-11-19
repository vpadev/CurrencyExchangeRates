using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowboy.APIService.Models.ViewModels
{
    public class CurrencyExchangeRateOnPeriodModel
    {
        public string SourceCurrencyCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Dictionary<DateTime, Dictionary<string, decimal>> ExchangeRates { get; set; }
    }
}
