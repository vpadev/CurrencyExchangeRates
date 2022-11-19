using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cowboy.APIService.Models
{
    [Table("CowboyDetails")]
    public class CowboyDetailsModel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public decimal Height { get; set; }
        public string? Hair { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Life_left { get; set; }

    }

    [Table("CowboyDetails")]
    public class CowboyDetailsInsertModel
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public decimal Height { get; set; }
        public string? Hair { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        [DefaultValue(100)]
        public decimal Life_left { get; set; }

    }
    public class PlayerGunDetailsModel
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public decimal Height { get; set; }
        public string? Hair { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Life_left { get; set; }
        public string? GunName { get; set; }
        public int Bulets_Left { get; set; }
    }

    public class GunDetailsModel
    {
        [Key]
        public int Id { get; set; }
        public string? GunName { get; set; }
        public int MaxNoOfBullets { get; set; }
        
    }

    public class CowboyDistanceModelModel
    {    
        public string? Distance { get; set; }
       
    }
    public class CowboyGunMappingModel
    {
        [Key]
        public int Id { get; set; }
        public int Person_Id { get; set; }
        public int Gun_Id { get; set; }
        public int Bulets_Left { get; set; }
    }
}