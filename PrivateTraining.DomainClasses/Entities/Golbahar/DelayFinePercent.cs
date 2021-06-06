using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("DelayFinePercents", Schema = "Golbahar")]
    public class DelayFinePercent :BaseEntity
    {
        #region Properties

        [Display(Name = "درصد")]
        public byte FinePercent { get; set; }

        [Display(Name = "تاریخ شروع")]
        [MaxLength(10)]
        public string FromTime { get; set; }

        [Display(Name = "تاریخ پایان")]
        [MaxLength(10)]
        public string ToTime { get; set; }

        [Display(Name = "کد نوع شرکت")]
        public int CompanyTypeId { get; set; }

        /// <summary>
        /// 0 =  درصدی
        /// 1 =  مبلغ 
        /// </summary>
        [Display(Name = "نوع جریمه")]
        public bool FineType { get; set; }

        [Display(Name = "مبلغ جریمه")]
        public double FinePrice { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("CompanyTypeId")]
        [InverseProperty("DelayFinePercents")]
        public virtual CompanyType CompanyTypes { get; set; }

        //  public virtual ICollection<Company> Companies { get; set; }

        #endregion
    }
}
