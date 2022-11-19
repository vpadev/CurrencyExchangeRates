using Cowboy.APIService.Models;


namespace Cowboy.APIService.Contracts
{
    public interface IApiRepository
    {
        List<PlayerGunDetailsModel> GetPlayerDetails();
        List<GunDetailsModel> GetGunDetails();
        bool UpdatePlayerDetails(CowboyDetailsModel cowboyDetails, int? gunID);
        int InsertPlayerDetails(CowboyDetailsInsertModel cowboyDetails, int gunID);
        bool DeleteCowboyDetails(int id);
        bool ShootTheGun(int CowBoyShooterID, int CowBoyTargetID);
        bool ReloadTheGun(int CowBoyID);
        List<CowboyDistanceModelModel> DistanceBetweenTwoCowboys(int CowBoyID1, int CowBoyID2);
        List<PlayerGunDetailsModel> CompareTwoCowboys(int CowBoyID1, int CowBoyID2);        
    }
}
