using PrivateTraining.DomainClasses.Entities.FrameWork;
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
    [Table("Programs", Schema = "Climbing")]

    public class Program : BaseEntity
    {

        #region Properties

        [Display(Name = " عنوان")]
        [Required]
        //   [MaxLength(100)]
        public string ProgramName { get; set; }

        [Display(Name = " منطقه")]
        [Required]
        [MaxLength(100)]
        public string AreaName { get; set; }

        [Display(Name = " عرض جغرافیایی")]
        [MaxLength(30)]
        public string Longitude { get; set; }

        [Display(Name = " طول جغرافیایی")]
        [MaxLength(30)]
        public string Latitude { get; set; }

        [Display(Name = " استان")]
        public int StateId { get; set; }

        [Display(Name = " شهر")]
        public int CityId { get; set; }

        /// <summary>
        /// 1= مذکر
        /// 2= مونث
        /// 3= هردو
        /// </summary>
        [Display(Name = " جنسیت")]
        public byte Sex { get; set; }

        [Display(Name = " هزینه")]
        [Required]
        public int Cost { get; set; }

        [Display(Name = "حداقل شرکت کننده")]
        //[Required]
        public byte MinUser { get; set; }

        [Display(Name = " حداکثر شرکت کننده")]
        [Required]
        public byte MaxUser { get; set; }

        [Display(Name = " نوع خودرو")]
        [MaxLength(50)]
        public string CarType { get; set; }

        [Display(Name = " مکان قرار")]
        [Required]
        [MaxLength(200)]
        public string Place { get; set; }

        /// <summary>
        /// 1= 1 روز 
        /// 2 = بیش از یک روز
        /// </summary>
        [Display(Name = " تعداد روز")]
        public byte CountDay { get; set; }

        [Display(Name = " ساعت شروع")]
        [Required]
        [MaxLength(6)]
        public string FromTime { get; set; }

        [Display(Name = " ساعت پایان")]
        [Required]
        [MaxLength(6)]
        public string ToTime { get; set; }

        [Display(Name = " تاریخ شروع برنامه")]
        [Required]
        [MaxLength(10)]
        public string FromDate { get; set; }

        [Display(Name = " تاریخ پایان برنامه")]
        [Required]
        [MaxLength(10)]
        public string ToDate { get; set; }

        //---------------------  مهلت ثبت نام
        [Display(Name = " ساعت شروع ثبت نام")]
        [Required]
        [MaxLength(6)]
        public string RegisterFromTime { get; set; }

        [Display(Name = " ساعت پایان ثبت نام")]
        [Required]
        [MaxLength(6)]
        public string RegisterToTime { get; set; }

        [Display(Name = " تاریخ شروع ثبت نام")]
        [Required]
        [MaxLength(10)]
        public string RegisterFromDate { get; set; }

        [Display(Name = " تاریخ پایان ثبت نام")]
        [Required]
        [MaxLength(10)]
        public string RegisterToDate { get; set; }

        //---------------------

        [Display(Name = " لوازم")]
        [MaxLength(1000)]
        public string Equipments { get; set; }

        [Display(Name = "سرپرست ")]
    //    [Required]
        [MaxLength(500)]
        public string ModratorNames { get; set; }

        [Display(Name = "امدادگر ")]
        [MaxLength(100)]
        public string Helper { get; set; }

        [Display(Name = "مسئول فنی ")]
        [MaxLength(100)]
        public string TechnicalAssistant { get; set; }

        [Display(Name = " توضیحات")]
        public string Description { get; set; }

        [Display(Name = " حداقل سن")]
      //  [Required]
        public byte MinAge { get; set; }

        [Display(Name = " حداکثر سن")]
      //  [Required]
        public byte MaxAge { get; set; }

        [Display(Name = "ستاره")]
        public byte Star { get; set; }

        [Display(Name = "علاقمندی")]
        public int Favorites { get; set; }

        [Display(Name = "بازدید")]
        public int Visited { get; set; }

        ///// <summary>
        ///// 1= در حال اجرا
        ///// 2= اجرا شده
        ///// 3= کنسل
        ///// </summary>
        //[Display(Name = "وضعیت برنامه")]
        //public byte ProgramStatus { get; set; }

        /// <summary>
        /// 0= عدم نمایش 
        /// 1= نمایش
        /// 2= Trash 
        /// 3= کنسل
        /// 4= حذف
        /// </summary>
        [Display(Name = "وضعیت نمایش")]
        public byte Status { get; set; }

        [Display(Name = "دلیل حذف یا کنسل")]
        [MaxLength(500)]
        public string RejectDesc { get; set; }

        [Display(Name = "تاریخ حذف یا کنسل")]
        [MaxLength(20)]
        public string RejectDate { get; set; }

        //[Display(Name = "حذف")]
        //public bool Deleted { get; set; }

        [Display(Name = " تاریخ ثبت رکورد")]
        [MaxLength(10)]
        public string SaveDate { get; set; }

        [Display(Name = " کاربر ثبت کننده")]
        public int UserId { get; set; }

        [Display(Name = " انتخاب باشگاه")]
        [Required]
        public int ClubId { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("CityId")]
        [InverseProperty("Programs")]
        public virtual BaseTable.City Cities { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Programs")]
        public virtual ApplicationUser Users { get; set; }

        [ForeignKey("ClubId")]
        [InverseProperty("Programs")]
        public virtual Club Clubs { get; set; }

        public virtual ICollection<ClimbingFile> ClimbingFiles { get; set; }
      //  public virtual ICollection<CommentCliming> Comments { get; set; }
        public virtual ICollection<PaymentOrderDetail> PaymentOrderDetails { get; set; }
        public virtual ICollection<ProgramModrator> ProgramModrators { get; set; }

        #endregion


    }
}
