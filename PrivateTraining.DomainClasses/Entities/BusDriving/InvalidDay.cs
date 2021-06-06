using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("InvalidDays", Schema = "BusDriving")]
    public class InvalidDay : BaseEntity
    {
        /// <summary>
        /// روزهایی که پرسنل نمی توانند مرخصی بگیرند
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string FromDate { get; set; }
        [MaxLength(10)]
        public string ToDate { get; set; }

    }
}
