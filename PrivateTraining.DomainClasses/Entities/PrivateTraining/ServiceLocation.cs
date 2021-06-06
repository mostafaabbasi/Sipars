using PrivateTraining.DomainClasses.Entities.BaseTable;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceLocations", Schema = "PrivateTraining")]
    public class View_ServiceLocations : BaseEntity
    {

        #region Properties

        [Display(Name = "کد محل")]
        [Required]
        public int LocationCode { get; set; }


        [Display(Name = "خدمت")]
        [Required]
        public int ServiceId { get; set; }

        [Display(Name = "شهر")]
        [Required]
        public int CityId { get; set; }


        [Display(Name = "محل")]
        [Required]
        public int LocationId { get; set; }

        [Display(Name = "کد خدمت")]
        [Required]
        public int ServiceCode { get; set; }

        [Display(Name = "کد محل-خدمت")]
        [Required]
        public string ServiceLocationCode { get; set; }

        [Display(Name = " حداقل ظرفیت خدمت دهنده")]
        [Required]
        public int MinCapacityService { get; set; }

        [Display(Name = "حداکثر ظرفیت خدمت دهنده")]
        [Required]
        public int MaxCapacityService { get; set; }

        [Display(Name = "درصد تعداد رزرو")]
      //  [Range(0, 100)]
        [Required]
        //   public int PercentCountReservationService { get; set; }
        [Range(-100, 100)]
        public double PercentCountReservationService { get; set; }

        [Display(Name = "درصد کاهش و افزایش قیمت هر واحد کار")]
        [Required]
        public double PercentPriceWorkUnitServiceLocation { get; set; }

        [Display(Name = "درصد کاهش و افزایش قیمت ثبت نام خدمت دهنده")]
        [Required]
        public double PercentPriceRegisterServiceProvider { get; set; }

        //[Display(Name = "ظرفیت خدمت دهنده")]
        //[Required]
        //public int CapacityServiceProvider { get; set; }

        [Display(Name = "درصد حق السهم شرکت")]
        //[Range(0, 100)]
        [Required]
        //   public int PercentOfShares { get; set; }
        [Range(-100, 100)]
        public double PercentOfShares { get; set; }

        //--------- Calculations

        [Display(Name = "قیمت ثبت نام خدمت دهنده محاسبه شده پس از اعمال درصد")]
        [Required]
        public double CalculationPriceRegisterServiceProvider { get; set; }

        //[Display(Name = "قیمت حق السهم شرکت محاسبه شده پس از اعمال درصد")]
        //[Required]
        //public double CalculationPricePercentOfShares { get; set; }



        //--------- Calculations


        #endregion

        #region Navigators

        [ForeignKey("ServiceId")]
        [InverseProperty("ServiceLocations")]
      //  public virtual Service Services { get; set; }
        public virtual ServiceProperties Services { get; set; }

        [ForeignKey("LocationId")]
        [InverseProperty("ServiceLocations")]
        public virtual Location Locations { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("ServiceLocations")]
        public virtual City Cities { get; set; }

        public virtual ICollection<ServiceLocationWorkUnit> ServiceLocationWorkUnits { get; set; }
        public virtual ICollection<UserServiceLocation> UserServiceLocations { get; set; }
        public virtual ICollection<ServiceReceiverServiceLocation> ServiceReceiverServiceLocations { get; set; }



        #endregion


    }
}
