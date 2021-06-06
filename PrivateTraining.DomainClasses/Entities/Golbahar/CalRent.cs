using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("CalRents", Schema = "Golbahar")]
    public class CalRent : BaseEntity
    {
        [Display(Name = "کد کاربر")]
        public int UserId { get; set; }

        [Display(Name = "مبلغ اجاره")]
        public double Price { get; set; }

        [Display(Name = "سال")]
        [MaxLength(4)]
        public string Year { get; set; }

        [Display(Name = "مبلغ اجاره سال قبل")]
        public double PYear { get; set; }

        [Display(Name = "سهم العرصه")]
        public double PortionField { get; set; }

        [Display(Name = "قیمت منطقه")]
        public double PriceOfArea { get; set; }

        [Display(Name = "درصد افزایش سالانه")]
        public Nullable<double> IncreasePercent { get; set; }

        [Display(Name = "تاریخ ثبت")]
        [MaxLength(20)]
        public string SaveDate { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        [MaxLength(20)]
        public string EditDate { get; set; }


        [ForeignKey("UserId")]
        [InverseProperty("CalRents")]
        public virtual Unit Units { get; set; }
    }
}
