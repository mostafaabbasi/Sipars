using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    // --  تعیین درصد یا مبلغ افزایش سالیانه
    [Table("PercentagIncreaseAnnuallys", Schema = "Golbahar")]
    public class PercentagIncreaseAnnually :BaseEntity
    {
        [Display(Name = "درصد")]
        public byte Percent { get; set; }

        [Display(Name = "سال")]
        [MaxLength(4)]
        public string Year { get; set; }


        /// <summary>
        /// 0 =  درصدی
        /// 1 =  مبلغ 
        /// </summary>
        [Display(Name = "نوع جریمه")]
        public bool Type { get; set; }

        [Display(Name = "مبلغ افزایش")]
        public double Price { get; set; }
    }
}
