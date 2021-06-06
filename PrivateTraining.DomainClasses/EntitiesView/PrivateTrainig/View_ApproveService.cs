using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class View_ApproveService
    {

        public string StateName { get; set; }

        public string CityName { get; set; }

        public string ServiceName { get; set; }

        public int LocationId { get; set; }
        public string LocationName { get; set; }

        public int ServicesProviderId { get; set; }
        public string ServiceProviderInfoName { get; set; }
        public string ServiceProviderInfoFamily { get; set; }

        //public List<Service> Services { get; set; }
        //public List<ServiceProviderInfo> ServiceProviderInfo { get; set; }
        public List<SelectServiceProviderForService> SelectServiceProviderForServices { get; set; }


        public ServiceProviderInfo ServiceProviderInfo { get; set; }
        public ServiceReceiverInfo ServiceReceiverInfo { get; set; }

        [Display(Name = "تلفن همراه")]
       // [Required]
        [MaxLength(11)]
        public string Mobile { get; set; }

        [Display(Name = "نام ")]
       // [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگي")]
      //  [Required]
        [MaxLength(100)]
        public string Family { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "جنسیت")]
       // [Required]
        public bool Sex { get; set; }

        [Display(Name = "تلفن منزل")]
        [MaxLength(500)]
        public string HomePhone { get; set; }

        [Display(Name = "آدرس منزل")]
        [MaxLength(500)]
        public string HomeAddress { get; set; }

        [Display(Name = "استان")]
       // [Required]
        public int StateId { get; set; }

        [Display(Name = "شهر")]
       // [Required]
        public int CityId { get; set; }

        [Display(Name = "خدمت")]
        // [Required]
        public int ServiceId { get; set; }

        [Display(Name = "خدمت دهنده")]
        // [Required]
        public int ServiceProviderId { get; set; }

        [Display(Name = "پلاک")]
        [MaxLength(10)]
        public string HomeNumber { get; set; }

        [Display(Name = "واحد آپارتمان")]
        public int UnitNumber { get; set; }


        public string DateReceiver { get; set; }
        public string TimeReceiver { get; set; }


    }

    public class SelectServiceProviderForService
    {
        public int ServiceProviderId { get; set; }
        public string ServiceProviderName { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int WorkUnitId { get; set; }
        public int ServiceLevelListId { get; set; }

    }

}
