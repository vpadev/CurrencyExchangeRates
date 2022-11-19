using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowboy.APIService.Models.ViewModels
{
    public class CurrencyExchangeRateModel
    {
        [JsonProperty("base")]
        public string SourceCurrencyCode { get; set; }
        
        [JsonProperty("rates")]
        public Dictionary<string, string> ExchangeRates { get; set; }
        
        [JsonProperty("date")]
        public DateTime RecordedOn { get; set; }
    }
}
