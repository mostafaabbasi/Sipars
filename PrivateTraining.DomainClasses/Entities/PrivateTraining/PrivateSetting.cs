using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("PrivateSettings", Schema = "PrivateTraining")]
    public class PrivateSetting : BaseEntity
    {
        [Display(Name = "نمایش پرداخت آنلاین")]
        public bool ShowPayOnline { get; set; }


    }
}
