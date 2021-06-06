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
    [Table("ClimbingFiles", Schema = "Climbing")]

    public class ClimbingFile : BaseEntity
    {

        public ClimbingFile()
        {
            Priority = 0;
        }

        #region Properties
        /// <summary>
        /// 1= Logo
        /// 2 = File 
        /// 3 = Pic
        /// </summary>
        [Display(Name = "نوع")]
        [Required]
        public byte FileType { get; set; }

        [Display(Name = "نام فایل")]
        [Required]
        public string FileName { get; set; }

        [Display(Name = "مسیر")]
        [Required]
        public string FilePath { get; set; }

        [Display(Name = "کد باشگاه")]
        public int ClubId { get; set; }

        [Display(Name = "کد کاربر")]
        public Nullable<int> UserId { get; set; }

        [Display(Name = "کد برنامه")]
        public Nullable<int> ProgramId { get; set; }

        [Display(Name = "اولویت")]
        public byte Priority { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("UserId")]
        [InverseProperty("ClimbingFiles")]
        public virtual ApplicationUser UserFs { get; set; }

        [ForeignKey("ProgramId")]
        [InverseProperty("ClimbingFiles")]
        public virtual Program ProgramFs { get; set; }

        //[ForeignKey("ClubId")]
        //[InverseProperty("ClimbingFiles")]
        //public virtual Club Clubs { get; set; }

        #endregion

    }
}
