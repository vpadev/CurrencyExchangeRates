using Cowboy.APIService.Contracts;
using Cowboy.APIService.Models;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Cowboy.APIService.DataAccess
{
    public class ApiRepository : IApiRepository
    {
        private readonly IDbConnection _connection;

        public ApiRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public ApiRepository()
        {
        }

        public List<PlayerGunDetailsModel> GetPlayerDetails()
        {
            try
            {
                const string sql = "select P.*,G.GunName,PG.Bulets_Left  from [CowBoyGunMapping] PG INNER JOIN CowboyDetails  P on P.Id=PG.Cowboy_Id INNER JOIN GunDetails  G on G.Id=PG.Gun_Id";
                var playerDetails = _connection.Query<PlayerGunDetailsModel>(sql);
                return playerDetails.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GunDetailsModel> GetGunDetails()
        {
            try
            {
                const string sql = "select *  from GunDetails";
                var gunDetails = _connection.Query<GunDetailsModel>(sql);
                return gunDetails.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool UpdatePlayerDetails(CowboyDetailsModel CowboyDetails, int? gunID)
        {
            try
            {
                _connection.Update(CowboyDetails);
                if (gunID > 0)
                {
                    var sqlbullet = "SELECT MaxNoOfBullets FROM GunDetails Where Id =" + gunID;
                    var bulletresult = _connection.Query<GunDetailsModel>(sqlbullet).FirstOrDefault();
                    int bulletsleft = bulletresult.MaxNoOfBullets;
                    var sql = "UPDATE CowBoyGunMapping set Gun_Id= " + gunID + ", Bulets_Left =" + bulletsleft + "  where Cowboy_Id= " + CowboyDetails.Id + "";
                    _connection.Query<CowboyGunMappingModel>(sql);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int InsertPlayerDetails(CowboyDetailsInsertModel CowboyDetails, int gunID)
        {
            try
            {
                var result = _connection.InsertAsync(CowboyDetails).Result;
                result = Convert.ToInt32(result);
                var sqlbullet = "SELECT MaxNoOfBullets FROM GunDetails Where Id =" + gunID;
                var bulletresult = _connection.Query<GunDetailsModel>(sqlbullet).FirstOrDefault();
                int bulletsleft = bulletresult.MaxNoOfBullets;
                var sql = "INSERT INTO CowBoyGunMapping VALUES (" + result + "," + gunID + "," + bulletsleft + ")";
                _connection.Query<CowboyGunMappingModel>(sql);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteCowboyDetails(int id)
        {
            try
            {
                var sqlmaster = "DELETE FROM CowBoyGunMapping Where Cowboy_Id =" + id;
                _connection.Query<CowboyDetailsModel>(sqlmaster);
                var sql = "DELETE FROM CowboyDetails Where Id =" + id;
                _connection.Query<CowboyDetailsModel>(sql);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ShootTheGun(int CowBoyShooterID, int CowBoyTargetID)
        {
            try
            {
                var procedure = "[ShootTheGunBetweenTwoCowboys]";
                var values = new { CowBoyShooterID = CowBoyShooterID, CowBoyTargetID = CowBoyTargetID };
                _connection.Query<CowboyDistanceModelModel>(procedure, values,
                             commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ReloadTheGun(int CowBoyID)
        {
            try
            {
                var sql = "Update CowBoyGunMapping set Bulets_Left=(select GD.MaxNoOfBullets from CowboyDetails PD inner join [CowBoyGunMapping] PG on PG.Cowboy_Id=PD.ID INNER JOIN GunDetails GD on GD.id=PG.Gun_Id Where PD.Id=" + CowBoyID + ") where Cowboy_Id = " + CowBoyID;
                _connection.Query<CowboyDetailsModel>(sql);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CowboyDistanceModelModel> DistanceBetweenTwoCowboys(int CowBoyID1, int CowBoyID2)
        {
            try
            {
                var procedure = "[GetDistanceBetweenTwoCowboys]";
                var values = new { Cowboy1 = CowBoyID1, Cowboy2 = CowBoyID2 };
                var results = _connection.Query<CowboyDistanceModelModel>(procedure, values,
                             commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<PlayerGunDetailsModel> CompareTwoCowboys(int CowBoyID1, int CowBoyID2)
        {
            try
            {
                var procedure = "[CompareTwoCowboys]";
                var values = new { Cowboy1 = CowBoyID1, Cowboy2 = CowBoyID2 };
                var results = _connection.Query<PlayerGunDetailsModel>(procedure, values,
                             commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}