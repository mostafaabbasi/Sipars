using PrivateTraining.DomainClasses.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Security
{
    [Table("SuspensionUsers", Schema = "Security")]
    public class SuspensionUser  : BaseEntity
    {

        [Display(Name = "تاریخ تعلیق ")]
        [MaxLength(10)]
        public string SuspensionDate { get; set; }

        [Display(Name = "توضیحات مربوط به تعلیق ")]
        [MaxLength(500)]
        public string SuspensionDesc { get; set; }

        [Required]
        [Display(Name = "تاریخ شروع تعلیق")]
        public string FromSuspensionDate { get; set; }

        [Required]
        [Display(Name = "تاریخ اتمام تعلیق")]
        public string ToSuspensionDate { get; set; }


        public int UserId { get; set; }

        #region Navigators

        [ForeignKey("UserId")]
        [InverseProperty("SuspensionUsers")]
        public virtual ApplicationUser Users { get; set; }

        #endregion

    }

}
