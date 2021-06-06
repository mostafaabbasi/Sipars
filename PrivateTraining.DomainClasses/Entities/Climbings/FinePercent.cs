using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Climbings
{
    [Table("FinePercents", Schema = "Climbing")]

    public class FinePercent :BaseEntity
    {
        /// <summary>
        /// 1= 1 روز 
        /// 2 = بیش از یک روز
        /// </summary>
        [Display(Name = " تعداد روز")]
        public byte CountDay { get; set; }

        [Display(Name = " تعداد روز باقیمانده")]
        public byte RemainingDay { get; set; }

        [Display(Name = " درصد")]
        public byte Percent { get; set; }

    }
}
