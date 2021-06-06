using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("UserFiles", Schema = "PrivateTraining")]
    public class UserFile : BaseEntity
    {
        [Required]
        // کارت ملی=0 , مدرک تحصیلی=1 ,مدرک فنی و حرفه ای=2,سایر مدارک=3
        [Display(Name = "نوع فایل ")]
        public byte FileType { get; set; }

        [Required]
        [Display(Name = "نام فایل ")]
        public string  FileName { get; set; }

        [Required]
        [Display(Name = "مسیر ذخیره فایل ")]
        public string PathDocumentations { get; set; }

        [Required]
        public int UserId { get; set; }

        #region Navigators

        [ForeignKey("UserId")]
        [InverseProperty("UserFiles")]
        public virtual ApplicationUser Users { get; set; }

        #endregion
    }
}
