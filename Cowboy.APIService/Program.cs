using Cowboy.APIService.DataAccess;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Register serilog here
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddSingleton(InitializeDb());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IApiRepository, ApiRepository>();
builder.Services.AddScoped<ICurrencyExchangeRateRepository, CurrencyExchangeRateRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.DefaultModelsExpandDepth(-1);
    });
    app.UseHsts();

}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();



IDbConnection InitializeDb()
{

    SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder();
    var Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Config");
    //Azure SQL Server Name 
    conn.DataSource = Config["DataSource"];
    //User to connect to Azure
    conn.UserID = Config["UserID"];
    //Password used in Azure
    conn.Password = Config["Password"];
    //Azure database name
    conn.InitialCatalog = Config["InitialCatalog"];

    //Connect local SQL data
    //SqlConnection conn = new SqlConnection("Server= SARATHPRAKASH; Database= DB_Cowboy; Integrated Security=True;");
    var connection = new SqlConnection(conn.ConnectionString);
    connection.Open();

    // Migrate up
    var assembly = typeof(Program).GetTypeInfo().Assembly;
    var migrationResourceNames = assembly.GetManifestResourceNames()
        .Where(x => x.EndsWith(".sql"))
        .OrderBy(x => x);
    if (!migrationResourceNames.Any()) throw new System.Exception("No migration files found!");
    foreach (var resourceName in migrationResourceNames)
    {
        var sql = GetResourceText(assembly, resourceName);
        var command = connection.CreateCommand();
        command.CommandText = sql;
        command.ExecuteNonQuery();
    }

    return connection;
}

string GetResourceText(Assembly assembly, string resourceName)
{
    using (var stream = assembly.GetManifestResourceStream(resourceName))
    {
        using (var reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }
}