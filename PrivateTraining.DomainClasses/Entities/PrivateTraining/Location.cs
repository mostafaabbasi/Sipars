using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("Locations", Schema = "PrivateTraining")]
    public class Location : BaseEntity
    {

        //#region Properties

        [Display(Name = "نام منطقه")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public int CityId { get; set; }

        //[MaxLength(5)]
        public int LocationCode { get; set; }

        [Display(Name = "درصد افزایش یا کاهش حق السهم شرکت")]
        // [Range(0, 100)]
        // public int PercentOfShares { get; set; }
        [Range(-100, 100)]
        public double PercentOfShares { get; set; }

        [Display(Name = "کاهش یا افزایش درصد قیمت ثبت نام خدمت دهنده")]
        // [Range(0, 100)]
        [Range(-100, 100)]
        public double PercentPriceRegisterServiceProvider { get; set; }

        [Display(Name = "کاهش یا افزایش درصد قیمت هر واحد کار")]
        //[Range(0, 100)]
        [Range(-100, 100)]
        public double PercentPriceWorkUnitServiceLocation { get; set; }



        //[Display(Name = "درصد افزایش یا کاهش مبلغ خدمت")]
        //[Range(0, 100)]
        //public int PercentOfPriceService { get; set; }

        //[Display(Name = "درصد افزایش یا کاهش مبلغ ثبت نام")]
        //[Range(0, 100)]
        //public int PercentOfPriceRegister { get; set; }

        //#endregion

        //#region Navigators

        [ForeignKey("CityId")]
        [InverseProperty("Locations")]
        public virtual City Cities { get; set; }

        //public virtual ICollection<ServiceNotInLocation> ServiceNotInLocations { get; set; }
        public virtual ICollection<View_ServiceLocations> ServiceLocations { get; set; }
      //  public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<UserServiceLocation> UserServiceLocations { get; set; }
        public virtual ICollection<UserLocation> UserLocations { get; set; }


        //#endregion


    }
}
