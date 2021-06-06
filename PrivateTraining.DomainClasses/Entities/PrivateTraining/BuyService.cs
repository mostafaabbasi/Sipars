using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    // ReSharper disable  InconsistentNaming
    [Table("BuyService", Schema = "PrivateTraining")]
    public class BuyService
    {
        #region Properties

        public int buyServiceId { get; set; }
        public int serviceId { get; set; }
        public int locationId { get; set; }
        public int userCityId { get; set; }
        public string userCityTitle { get; set; }
        public string userAddress { get; set; }
        public string userMobile { get; set; }
        public string userDescription { get; set; }
        public string providerServiceLocationStatus { get; set; }
        public string attachmentPath { get; set; }
        public string providerType { get; set; }
        public string workPriceListJson { get; set; }

        public string date { get; set; }
        public string time { get; set; }
        public string timeRegister { get; set; }
        public bool dateTimeSyncByProvider { get; set; }

        //not use in providerSelect
        public int serviceLevelListId { get; set; }

        public string statusChangeJson { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required] public int serviceLocationId { get; set; }
        [Required] public int serviceReceiverId { get; set; }
        [Required] public int serviceProviderId { get; set; }
        [Required] public string dateRegister { get; set; }

        public int status { get; set; }
        public long payed { get; set; } = 0;
        public long payedOffline { get; set; } = 0;

        public int chatReadProvider { get; set; }
        public int chatReadCustomer { get; set; } 
        
        #endregion
    }
    
    public static class BuyServiceStatusEnum
    {
        public static int pending = 0;
        public static int accept = 1;
        public static int certain = 2;
        public static int unfinished = 3;
        public static int final = 4;
        public static int checking = 5;
        public static int unCertain = 6;
        public static int rejected = 7;
        public static int canceled = 8;
        public static int deleted = 9;
        public static int doing = 10;
        public static int finish = 12;
    }
}