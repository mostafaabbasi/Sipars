using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Climbings
{
    [Table("Clubs", Schema = "Climbing")]

    public class Club : BaseEntity
    {
        //public Club()
        //{
        //    Deleted = false;
        //}

        #region Properties

        [Display(Name = "نام باشگاه")]
        [Required]
        [MaxLength(100)]
        public string ClubName { get; set; }

        [Display(Name = "شماره ثبت")]
        [Required]
        [MaxLength(100)]
        public string RegistrationNumber { get; set; }

        [Display(Name = " تاریخ تأسیس")]
        [MaxLength(10)]
        public string FoundationDate { get; set; }

        [Display(Name = " شماره کارت بانکی")]
        [Required]
        [MaxLength(50)]
        public string BankCards { get; set; }

        [Display(Name = " آدرس")]
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }

        [Display(Name = " تلفن")]
        [Required]
        [MaxLength(20)]
        public string Phone { get; set; }

        [Display(Name = " استان")]
        public int StateId { get; set; }

        [Display(Name = " شهر")]
        public int CityId { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }

        //[Display(Name = "حذف")]
        //public bool Deleted { get; set; }

        [Display(Name = " تاریخ ثبت رکورد")]
        public string SaveDate { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("CityId")]
        [InverseProperty("Clubs")]
        public virtual BaseTable.City Cities { get; set; }

        public virtual ICollection<ClubManager> ClubManagers { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
        //public virtual ICollection<ClimbingFile> ClimbingFiles { get; set; }

        #endregion

    }
}
