// See https://aka.ms/new-console-template for more information
using Cowboy.APIService.Contracts;
using Cowboy.APIService.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

DateTime? date = null;
if (args.Length == 0)
    return;

var serviceProvider = new ServiceCollection()
            .AddSingleton<ICurrencyExchangeRateRepository, CurrencyExchangeRateRepository>()
            .BuildServiceProvider();

var operationType = args[0];
if (operationType == "get")
{
    var sCode = args[1];
    var tCode = args[2];
    var amount = Convert.ToInt64(args[3]);
    if (args.Length > 4)
    {
        date = Convert.ToDateTime(args[4]);
    }

    var repository = serviceProvider.GetService<ICurrencyExchangeRateRepository>();

    var model = await repository.GetCurrencyExchangeRate(sCode, tCode, amount, date);
    var result = JsonConvert.SerializeObject(model, Formatting.Indented);
    Console.WriteLine($"Exchange Rate are \n {result}");
    Console.ReadKey();
}
else
{
    //save currency exchange rates
}