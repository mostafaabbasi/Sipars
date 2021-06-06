using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.BuilderProperties;
using PrivateTraining.DomainClasses.Entities.Security;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using PrivateTraining.DomainClasses.Entities.BusDriving;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DomainClasses.Entities.Climbings;

namespace PrivateTraining.DomainClasses.Security
{
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        #region Property

        /// <summary>
        /// 0= غیر فعال
        /// 1= فعال
        /// </summary>
        [Display(Name = "وضعیت ")]
        public bool Status { get; set; }

        [Display(Name = "حذف منطقی ")] public byte Deleted { get; set; }

        [Display(Name = "کد کاربر")] public int PersonnelId { get; set; }

        /// <summary>
        /// notuser=0;
        ///  ServiceProvider = 1,
        ///  ServiceReceiver = 2,
        /// </summary>
        [Display(Name = "نقش کاربر")]
        public byte UserType { get; set; }

        [Display(Name = "نام ")]
        //[Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگي")]
        //[Required]
        [MaxLength(100)]
        public string Family { get; set; }

        // //[Required]
        [Display(Name = "ایمیل")] public string Email { get; set; }

        [Display(Name = "شماره همراه")]
        //[Required]
        [MaxLength(11)]
        public string Mobile { get; set; }

        [Display(Name = "آدرس منزل")]
        [MaxLength(500)]
        public string HomeAddress { get; set; }

        [Display(Name = "جنسیت")]
        //[Required]
        public bool Sex { get; set; }

        [Display(Name = "تلفن منزل")]
        [MaxLength(11)]
        public string HomePhone { get; set; }

        [Display(Name = "کد ملی")] public string NationalCode { get; set; }

        [Display(Name = "استان")] public int StateId { get; set; }

        [Display(Name = "شهر")] public Nullable<int> CityId { get; set; }

        [Display(Name = "مسیر ذخیره فایل")] public string Path { get; set; }

        [Display(Name = "عکس")] public string Picture { get; set; }

        [Display(Name = "تاریخ تولد")] public string BrithDay { get; set; }

        [Display(Name = "سال تولد")]
        //[Required]
        [MaxLength(4)]
        public string YearBrithDay { get; set; }

        [Display(Name = "ماه تولد")]
        //[Required]
        [MaxLength(2)]
        public string MonthBrithDay { get; set; }

        [Display(Name = "روز تولد")]
        //[Required]
        [MaxLength(2)]
        public string DayBrithDay { get; set; }

        [Display(Name = "تاریخ عضویت")] public string RegisterDate { get; set; }

        [Display(Name = "کد فعالسازی")] public string ActiveCode { get; set; }

        [Display(Name = "آدرس ها")] public string AddressJson { get; set; }

        [Display(Name = "اعتبار")] public long Credit { get; set; }
        [Display(Name = "Subscription")] public string Subscription { get; set; }

        //[Display(Name = "تاریخ آخرین لاگین")]
        //[MaxLength(10)]
        //public string LastLoginDate { get; set; }

        //[Display(Name = "تعداد لاگین")]
        //public int CountLogin { get; set; }

        #endregion

        #region navigation

        [ForeignKey("CityId")]
        //[InverseProperty("ApplicationUsers")]
        public virtual City Cities { get; set; }

        [ForeignKey("StateId")]
        [InverseProperty("ApplicationUsers")]
        public virtual State States { get; set; }

        public virtual ICollection<GroupPolicyUser> GroupPolicyUsers { get; set; }
        public virtual ICollection<AccessLevelUser> AccessLevelUsers { get; set; }
        public virtual ICollection<SuspensionUser> SuspensionUsers { get; set; }

        #region Bus

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<ReferenceLeave> ReferenceLeaves { get; set; }
        public virtual ICollection<ReferenceLeave> senderReferenceLeaves { get; set; }
        public virtual ICollection<UserRequest> UserRequests { get; set; }
        public virtual ICollection<LeaveRequest> UserLeaveRequests { get; set; }

        #endregion

        #region PrivateTraining

        public virtual ICollection<UserService> UserServices { get; set; }
        public virtual ICollection<UserLocation> UserLocations { get; set; }
        public virtual ICollection<payment> Payments { get; set; }
        public virtual ICollection<paymentDetail> PaymentDetails { get; set; }
        public virtual ICollection<ServiceReceiverServiceLocation> ServiceReceiverServiceLocations { get; set; }
        public virtual ICollection<UserServiceLocation> UserServiceLocations { get; set; }
        public virtual ICollection<UserFile> UserFiles { get; set; }
        public virtual ICollection<FormAnswer> FormAnswers { get; set; }
        public virtual ICollection<UserServiceScore> UserServiceScores { get; set; }
        public virtual ICollection<Entities.PrivateTraining.CommentPrivate> CommentPrivates { get; set; }

        #endregion

        #region Climing

        //   public virtual ICollection<Entities.Climbings.CommentCliming> CommentClims { get; set; }
        public virtual ICollection<ClubManager> ClubManagers { get; set; }
        public virtual ICollection<Program> Programs { get; set; }
        public virtual ICollection<ClimbingFile> ClimbingFiles { get; set; }
        public virtual ICollection<ProgramModrator> ProgramModrators { get; set; }

        #endregion

        #region Framwork

        public virtual ICollection<PaymentOrder> PaymentOrders { get; set; }
        public virtual ICollection<PaymentOrderDetail> PaymentOrderDetails { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Freind> Freinds { get; set; }

        #endregion


        // public virtual ICollection<ProgramRegister> ProgramRegisters { get; set; }

        #endregion
    }
}