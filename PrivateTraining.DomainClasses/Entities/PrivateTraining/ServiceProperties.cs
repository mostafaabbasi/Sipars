using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServicesProperties", Schema = "PrivateTraining")]
    public class ServiceProperties : Service
    {
        [Display(Name = "کد خدمت")]
        [Required]
        //[MaxLength(4)]
        //[MinLength(4)]
        public int ServiceCode { get; set; }

        [Display(Name = "حداقل ظرفیت خدمت دهنده")]
        [Required]
        public int MinCapacity { get; set; }

        [Display(Name = " حداکثر ظرفیت خدمت دهنده")]
        [Required]
        public int MaxCapacity { get; set; }

        [Display(Name = "درصد تعداد رزرو")]
        [Range(0, 100)]
        [Required]
        public int PercentCountReservation { get; set; }

        //[Display(Name = "قیمت هر واحد کار")]
        //[Required]
        //public double PriceWorkUnit { get; set; }

        [Display(Name = "قیمت ثبت نام خدمت دهنده")]
        [Required]
        public double PriceRegisterServiceProvider { get; set; }

        //[Display(Name = "ظرفیت خدمت دهنده")]
        //[Required]
        //public int CapacityServiceProvider { get; set; }

        [Display(Name = "درصد حق السهم شرکت")]
        [Range(0, 100)]
        [Required]
        public int PercentOfShares { get; set; }

        // شرکتی:0 ---- شخصی:1 ---- هردو:2
        [Display(Name = "نحوه انجام خدمت")]
        [Required]
        public byte HowPerform { get; set; }


        [Display(Name = "بازدید اولیه")]
        [Required]
        public bool InitialVisit { get; set; }

        public string DescriptionUploud1 { get; set; }

        public string DescriptionUploud2 { get; set; }

        public string DescriptionUploud3 { get; set; }


        public string image { get; set; }
        public string tagTitle { get; set; }
        public bool showTag { get; set; }
//
//        askCustomerSex: true
//        askProviderAddress: true
//        askProviderSex: true
//        askTime: true
//        attach: true
//        multiPrice: true
//        pay: true
//        price: true
//        providerChoose: true
//        serviceLocation: true
        public bool askCustomerSex { get; set; }
        public bool askProviderAddress { get; set; }
        public bool askProviderSex { get; set; }
        public bool askTime { get; set; }
        public bool forceAttach { get; set; }
        public bool multiPrice { get; set; }

        public bool pricingSipars { get; set; }
        public bool pricingProvider { get; set; }
        public bool pricingShared { get; set; }

        public bool providerSelectSipars { get; set; }
        public bool providerSelectCustomer { get; set; }
        public bool providerSelectProvider { get; set; }


        public bool serviceLocationCustomer { get; set; }
        public bool serviceLocationProvider { get; set; }
        public bool serviceLocationLess { get; set; }

        public bool multiProviderSelect { get; set; }
        public bool multiProviderOffer { get; set; }

        public bool payOnline { get; set; }
        public int payMin { get; set; }
        public int payMinPercent { get; set; }



        public string serviceUnitName { get; set; }
        public int baseOff { get; set; }
        public int minOff { get; set; }
        public string serviceDescription { get; set; }
        public string priceDescription { get; set; }

//
        [JsonIgnore]
        public virtual ICollection<ServiceWorkUnit> ServiceWorkUnits { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<UserService> UserServices { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<FormAnswer> FormAnswers { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<UserServiceScore> UserServiceScores { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<View_ServiceLocations> ServiceLocations { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<ServiceLevelList> ServiceLevelLists { get; set; }
    }
}