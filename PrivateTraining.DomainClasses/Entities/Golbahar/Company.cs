using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("Companies", Schema = "Golbahar")]
    public class Company :BaseEntity
    {
        #region Properties

        [Display(Name = "کد شرکت")]
        [MaxLength(20)]
        public string CompanyCode { get; set; }

        [Display(Name = "نام شرکت")]
        [MaxLength(50)]
        public string CompanyName { get; set; }
        
        [Display(Name = "ایمیل")]
        [MaxLength(50)]
        public string CompanyEmail { get; set; }

        [Display(Name = "شماره تماس")]
        [MaxLength(50)]
        public string CompanyPhone { get; set; }

        [Display(Name = "آدرس دفتر")]
        [MaxLength(50)]
        public string CompanyAddress { get; set; }

        [Display(Name = "کد نوع شرکت")]
        public int CompanyTypeId { get; set; }

        //[Display(Name = "کد درصد جریمه دیرکرد")]
        //public int FinePercentId { get; set; }

        [Display(Name = "کدنوسازی")]
        [MaxLength(50)]
        public string RenovationCode { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("CompanyTypeId")]
        [InverseProperty("Companies")]
        public virtual CompanyType CompanyTypes { get; set; }

        //[ForeignKey("FinePercentId")]
        //[InverseProperty("Companies")]
        //public virtual DelayFinePercent DelayFinePercents { get; set; }

        public virtual ICollection<Project> Projects { get; set; }

        #endregion

    }
}
