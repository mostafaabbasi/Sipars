using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    //------ درصد خوش حسابی
    [Table("AccountPercents", Schema = "Golbahar")]
    public class AccountPercent : BaseEntity
    {
        [Display(Name = "درصد")]
        public byte Percent { get; set; }

        [Display(Name = "تاریخ شروع")]
        [MaxLength(10)]
        public string FromTime { get; set; }

        [Display(Name = "تاریخ پایان")]
        [MaxLength(10)]
        public string ToTime { get; set; }

        [Display(Name = "کد نوع شرکت")]
        public int CompanyTypeId { get; set; }

        [Display(Name = "کد شرکت")]
        public int CompanyId { get; set; }

        //-- کد مجتمع
        [Display(Name = "کد پروژه")]
        public int ProjectId { get; set; }

        [Display(Name = "کد بلوک")]
        public int BlocId { get; set; }

    }
}
