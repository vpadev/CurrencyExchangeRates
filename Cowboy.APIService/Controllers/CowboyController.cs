
using static System.Reflection.Metadata.BlobBuilder;
using System.Collections.Generic;

namespace Cowboy.APIService.Controllers
{
    public class CowboyController : Controller
    {
        private readonly IApiRepository apiRepository;
        private readonly ILogger<CowboyController> logger;

        public CowboyController(IApiRepository _apiRepository, ILogger<CowboyController> _logger)
        {
            apiRepository = _apiRepository;
            logger= _logger;
        }
    
        [HttpGet]
        [Route("CowBoyDetails")]
        public IEnumerable<PlayerGunDetailsModel> GetPlayerDetails()
        {
            try
            {
                logger.LogInformation("Get cowboys details executing");
                var playerDetails = apiRepository.GetPlayerDetails();
                return playerDetails;
            }
            catch (Exception ex)
            {
                logger.LogError("Get cowboys details failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("GunDetails")]
        public IEnumerable<GunDetailsModel> GetGunDetails()
        {
            try
            {
                logger.LogInformation("GetGunDetails executing");
                var gunDetails = apiRepository.GetGunDetails();
                return gunDetails;
            }
            catch (Exception ex)
            {
                logger.LogError("GetGunDetails failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("CreateCowboy{gunID}")]
        public string CreateCowboy([FromBody] CowboyDetailsInsertModel cowboy, int gunID)
        {
            try
            {
                logger.LogInformation("CreateCowboy executing");
                apiRepository.InsertPlayerDetails(cowboy, gunID);
                return "Cowboy created succesfully";
            }
            catch (Exception ex)
            {
                logger.LogError("CreateCowboy failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpPut]
        [Route("PatchCowboy")]
        public string PatchCowboy([FromBody] CowboyDetailsModel person, int? gunID = 0)
        {
            try
            {
                logger.LogInformation("PatchCowboy executing");
                var list = GetPlayerDetails().ToList();
                int index = list.FindIndex(item => item.Id == person.Id);
                var gunList = GetGunDetails().ToList();
                int gunIndex = gunList.FindIndex(item => item.Id == gunID);
                if (gunID != 0 && gunIndex == -1)
                {
                    return "Gun Id does not exist";
                }
                else if (index >= 0)
                {
                    apiRepository.UpdatePlayerDetails(person, gunID);
                    return "Patch Cowboy Success";
                }
                else
                {
                    return "Cowboy Id does not exist";
                }
            }
            catch (Exception ex)
            {
                logger.LogError("PatchCowboy failed");
                throw new Exception(ex.Message);
            }
        }


        [HttpDelete("DeleteCowBoy{Id}")]

        public string DeleteCowboy(int Id)
        {
            try
            {
                logger.LogInformation("DeleteCowboy executing");
                var list = GetPlayerDetails().ToList();
                int index = list.FindIndex(item => item.Id == Id);
                if (index >= 0)
                {
                    apiRepository.DeleteCowboyDetails(Id);
                    return "Delete Cowboy Success";
                }
                else
                {
                    return "Cowboy Id does not exist";
                }
            }
            catch (Exception ex)
            {
                logger.LogError("DeleteCowboy failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("ShootTheGun{CowBoyShooterID}/{CowBoyTargetID}")]
        public string ShootTheGun(int CowBoyShooterID, int CowBoyTargetID)
        {
            try
            {
                logger.LogInformation("ShootTheGun executing");
                var list = GetPlayerDetails().ToList();
                int shooterindex = list.FindIndex(item => item.Id == CowBoyShooterID);
                int targetindex = list.FindIndex(item => item.Id == CowBoyTargetID);
                if(shooterindex >=0 && targetindex>=0)
                {
                    apiRepository.ShootTheGun(CowBoyShooterID, CowBoyTargetID);
                    return "Shoot triggered between Cowboys";
                }
                else 
                {
                    return "Cowboy Id does not exist";
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError("ShootTheGun failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [Route("ReloadTheGun{CowBoyId}")]
        public string ReloadTheGun(int CowBoyId)
        {
            try
            {
                logger.LogInformation("ReloadTheGun executing");
                var list= GetPlayerDetails().ToList();
                int index = list.FindIndex(item => item.Id == CowBoyId);
                if (index >= 0)
                {
                    apiRepository.ReloadTheGun(CowBoyId);
                    return "Reloaded Gun Success";
                }
                else
                {
                    return "Cowboy Id does not exist";
                }
                
            }
            catch (Exception ex)
            {
                logger.LogError("ReloadTheGun failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("DistanceBetweenTwoCowboys{CowBoyID1}/{CowBoyID2}")]
        public IEnumerable<CowboyDistanceModelModel>? GetDistanceBetweenTwoCowboys(int CowBoyID1, int CowBoyID2)
        {
            try
            {
                logger.LogInformation("GetDistanceBetweenTwoCowboys executing");
                CowboyDistanceModelModel c = new CowboyDistanceModelModel();
                var list = GetPlayerDetails().ToList();
                int CowBoy1index = list.FindIndex(item => item.Id == CowBoyID1);
                int CowBoy2index = list.FindIndex(item => item.Id == CowBoyID2);
                if (CowBoy1index >= 0 && CowBoy2index >= 0)
                {
                    var distanceBetweenTwoCowboys = apiRepository.DistanceBetweenTwoCowboys(CowBoyID1, CowBoyID2);
                    return distanceBetweenTwoCowboys;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("GetDistanceBetweenTwoCowboys failed");
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("CompareTwoCowboys{CowBoyID1}/{CowBoyID2}")]
        public IEnumerable<PlayerGunDetailsModel> CompareTwoCowboys(int CowBoyID1, int CowBoyID2)
        {
            try
            {
                logger.LogInformation("CompareTwoCowboys executing");
                var distanceBetweenTwoCowboys = apiRepository.CompareTwoCowboys(CowBoyID1, CowBoyID2);
                return distanceBetweenTwoCowboys;
            }
            catch (Exception ex)
            {
                logger.LogError("CompareTwoCowboys failed");
                throw new Exception(ex.Message);
            }
        }
    }
}
