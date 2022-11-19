using Cowboy.APIService.DataAccess;
using Cowboy.APIService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace Cowboy.Xunit.Test
{
    public class CowboyTest
    {
        [Fact]
        public void CreatePatchDeleteCowboy()
        {

            try
            {
                CowboyDetailsInsertModel CowboyDetails = new CowboyDetailsInsertModel();
                CowboyDetailsModel CowboyEditDetails = new CowboyDetailsModel();
                ApiRepository api = new ApiRepository(InitializeDb());
                CowboyDetails.Name = "TestCowboyUser";
                CowboyDetails.Age = 30;
                CowboyDetails.Height = 5.22M;
                CowboyDetails.Hair = "Black";
                CowboyDetails.Longitude = 60.43M;
                CowboyDetails.Latitude = 160.43M;
                CowboyDetails.Life_left = 100M;

                var insertresult = api.InsertPlayerDetails(CowboyDetails, 2);
                Assert.True(IsNumeric(insertresult));

                CowboyEditDetails.Id = insertresult;
                CowboyEditDetails.Name = "TestCowboyEdit";
                CowboyEditDetails.Age = 30;
                CowboyEditDetails.Height = 5.22M;
                CowboyEditDetails.Hair = "Black";
                CowboyEditDetails.Longitude = 60.43M;
                CowboyEditDetails.Latitude = 160.43M;
                CowboyEditDetails.Life_left = 100M;

                bool updateresult = api.UpdatePlayerDetails(CowboyEditDetails, 2);
                Assert.True(updateresult);
                bool deleteresult = api.DeleteCowboyDetails(insertresult);
                Assert.True(deleteresult);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool IsNumeric(int insertresult)
        {
            if (insertresult > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private IDbConnection InitializeDb()
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
            var connection = new SqlConnection(conn.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}