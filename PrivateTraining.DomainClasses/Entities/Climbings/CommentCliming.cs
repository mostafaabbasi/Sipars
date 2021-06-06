using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Climbings
{
    [Table("Comments", Schema = "Climbing")]

    public class Comment : BaseEntity
    {
        #region Properties

        [Display(Name = "موضوع")]
        [MaxLength(100)]
        public string CommentSubject { get; set; }

        [Display(Name = "متن")]
        [Required]
        [MaxLength(500)]
        public string CommentDesc { get; set; }

        /// <summary>
        /// 1= شگفت
        /// 2= خوب
        /// 3= معمولی
        /// 4= توصیه نمیکنم
        /// </summary>
        [Display(Name = "ارزیابی")]
        public byte Assessment        { get; set; }

        [Display(Name = "سرپرستی")]
        public byte Supervision { get; set; }

        [Display(Name = "برنامه ریزی باشگاه")]
        public byte PlanningClub { get; set; }

        [Display(Name = "نحوه اجرا")]
        public byte Implementation { get; set; }

        [Display(Name = "سرویس حمل و نقل")]
        public byte TransportService { get; set; }

        public int ProgramId { get; set; }

        [Display(Name = "تاریخ")]
        [MaxLength(10)]
        public string CommentDate { get; set; }

        [Display(Name = "تاریخ")]
        [MaxLength(10)]
        public string CommentTime { get; set; }

        public Nullable<int> UserId { get; set; }

        #endregion

        #region Navigators

        //[ForeignKey("UserId")]
        //[InverseProperty("CommentClimings")]
        //public virtual ApplicationUser UserComments { get; set; }

        //[ForeignKey("ProgramId")]
        //[InverseProperty("CommentClimings")]
        //public virtual Program ProgramComments { get; set; }

        #endregion

    }
}
