using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("CalRentLogs", Schema = "Golbahar")]

    public class CalRentLog : BaseEntity
    {

        //-----------------  قبل از ویرایش

        [Display(Name = "کد کاربر")]
        public Nullable<int> UserId { get; set; }

        [Display(Name = "مبلغ اجاره")]
        public Nullable<double> Price { get; set; }

        [Display(Name = "سال")]
        [MaxLength(4)]
        public string Year { get; set; }

        [Display(Name = "مبلغ اجاره سال قبل")]
        public Nullable<double> PYear { get; set; }

        [Display(Name = "سهم العرصه")]
        public Nullable<double> PortionField { get; set; }

        [Display(Name = "قیمت منطقه")]
        public Nullable<double> PriceOfArea { get; set; }

        [Display(Name = "درصد افزایش سالانه")]
        public Nullable<double> IncreasePercent { get; set; }

        //-------------------------- بعد از ویرایش

        [Display(Name = "کد کاربر")]
        public Nullable<int> UserIdEdit { get; set; }

        [Display(Name = "مبلغ اجاره")]
        public Nullable<double> PriceEdit { get; set; }

        [Display(Name = "سال")]
        [MaxLength(4)]
        public string YearEdit { get; set; }

        [Display(Name = "مبلغ اجاره سال قبل")]
        public Nullable<double> PYearEdit { get; set; }

        [Display(Name = "سهم العرصه")]
        public Nullable<double> PortionFieldEdit { get; set; }

        [Display(Name = "قیمت منطقه")]
        public Nullable<double> PriceOfAreaEdit { get; set; }

        [Display(Name = "درصد افزایش سالانه")]
        public Nullable<double> IncreasePercentEdit { get; set; }

        //------------------------------------

        [Display(Name = "تاریخ ثبت")]
        [MaxLength(20)]
        public string SaveLogDate { get; set; }

        [Display(Name = "کد اجاره ای که قبلا ثبت شده")]
        public int CalRentId { get; set; }


 

    }
}

