// See https://aka.ms/new-console-template for more information
using Cowboy.APIService.Contracts;
using Cowboy.APIService.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

DateTime? date = null;
if (args.Length == 0)
    return;

var serviceProvider = new ServiceCollection()
            .AddSingleton(InitializeDb())
            .AddSingleton<ICurrencyExchangeRateRepository, CurrencyExchangeRateRepository>()
            .BuildServiceProvider();

var repository = serviceProvider.GetService<ICurrencyExchangeRateRepository>();

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

    var model = await repository.GetCurrencyExchangeRate(sCode, tCode, amount, date);
    var result = JsonConvert.SerializeObject(model, Formatting.Indented);
    Console.WriteLine($"Exchange Rate are \n {result}");
    Console.ReadKey();
}
else if (operationType == "save")
{
    //save currency exchange rates
    bool result = false;
    var sCode = args[1];
    var tCode = args[2];
    try
    {
        result = await repository.SaveCurrencyExchangeRate(sCode, tCode);
    }
    catch (Exception ex)
    {
        Console.WriteLine("save operation failed : \n\t" + ex);
    }
    if (result)
        Console.WriteLine("Exchange rates saved succesfully");
    else
        Console.WriteLine("Exchange rates creation failed - nothing got updated");
}


IDbConnection InitializeDb()
{

    SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();

    //Azure SQL Server Name 
    conn.DataSource = "sarathsqlserver1.database.windows.net";
    //User to connect to Azure
    conn.UserID = "sarathadmin";
    //Password used in Azure
    conn.Password = "Password123";
    //Azure database name
    conn.InitialCatalog = "sarathdatabase";

    //Connect local SQL data
    //SqlConnection conn = new SqlConnection("Server= SARATHPRAKASH; Database= DB_Cowboy; Integrated Security=True;");
    var connection = new SqlConnection(conn.ConnectionString);
    connection.Open();

    return connection;
}