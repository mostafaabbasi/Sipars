using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel 
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        //[Display(Name = "Email")]
        //[EmailAddress]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public SupplementaryInfoUser SupplementaryInfoUsers { get; set; }

        [Display(Name = "باقیمانده مرخصی ")]
        public int Remainingleave { get; set; }

        public int Id { get; }

        public byte RoleId { get; set; }

        [Display(Name = "حذف منطقی ")]
        public byte Deleted { get; set; }

        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "كد ملي")]
        [Required]
        [MaxLength(10)]
        public string NationalCode { get; set; }

        [Display(Name = "کد پرسنلی")]
        [Required]
        public int PersonnelId { get; set; }

        [Display(Name = "نام ")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگي")]
        [Required]
        [MaxLength(100)]
        public string Family { get; set; }

        [Display(Name = "تلفن همراه")]
        [MaxLength(11)]
        public string Mobile { get; set; }

        [Display(Name = "نشانی")]
        [MaxLength(500)]
        public string Address { get; set; }

        [Display(Name = "جنسیت")]
        public bool Sex { get; set; }

        [Display(Name = "نام پدر")]
        [MaxLength(50)]
        public string FatherName { get; set; }

        [Display(Name = "شماره شناسنامه")]
        [MaxLength(20)]
        public string ShId { get; set; }

        [Display(Name = "محل تولد")]
        [MaxLength(50)]
        public string PlaceOfBirth { get; set; }

        [Display(Name = "تلفن")]
        [MaxLength(11)]
        public string Phone { get; set; }

        [Display(Name = "تاريخ تولد")]
        [MaxLength(10)]
        public string BrithDay { get; set; }

        [Display(Name = "شماره گواهینامه")]
        [MaxLength(15)]
        public string CertificateId { get; set; }

        [Display(Name = "نوع گواهینامه")]
        [MaxLength(100)]
        public string CertificateType { get; set; }

        [Display(Name = "تاریخ صدور گواهینامه ")]
        [MaxLength(10)]
        public string CertificationDate { get; set; }

        [Display(Name = "تاریخ اعتبار گواهینامه ")]
        [MaxLength(10)]
        public string CertificateCredit { get; set; }

        [Display(Name = "مفقود/صدور ")]
        [MaxLength(50)]
        public string Status { get; set; }

        [Display(Name = "شماره اتوبوس ثابت ")]
        public int BusId { get; set; }

        [Display(Name = "سال استخدام ")]
        [MaxLength(10)]
        public string YearEmployment { get; set; }

        [Display(Name = "آموزش بدو خدمت ")]
        [MaxLength(100)]
        public string EducationComers { get; set; }

        [Display(Name = "سایر دوره های آموزشی ")]
        [MaxLength(200)]
        public string OtherCourses { get; set; }

        [Display(Name = "تعداد اولاد ")]
        public byte NumberChildren { get; set; }

        [Display(Name = "مدرک تحصیلی ")]
        [MaxLength(100)]
        public string Degree { get; set; }

        [Display(Name = "رشته تحصیلی ")]
        [MaxLength(100)]
        public string FieldOfStudy { get; set; }

        [Display(Name = "تاریخ صدور کارت سلامت ")]
        [MaxLength(10)]
        public string IssuedOnHealthCards { get; set; }

        [Display(Name = "مدت اعتبار ")]
        [MaxLength(50)]
        public string ValidityDuration { get; set; }

        [Display(Name = "مدت اعتبار به سال ")]
        public byte TheValidityPeriodOfTheYear { get; set; }

        [Display(Name = "تاریخ انقضاء ")]
        [MaxLength(10)]
        public string ExpirationDate { get; set; }

        [Display(Name = "عکس")]
        public string Picture { get; set; }

    }

    public class ResetPasswordViewModel
    {

        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        //[Required]
        //[EmailAddress]
        //[Display(Name = "ایمیل")]
        //public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "پسورد")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = " تکرار پسورد")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        //[Required]
       // [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "شماره همراه")]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

    }

    public class ChangePasswordsViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = " کلمه عبور جاری")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور جدید ")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "  تکرار کلمه عبور جدید")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterUserViewModel
    {
        #region Property
        public ServiceProviderInfo ServiceProviderInfo { get; set; }
        public ServiceReceiverInfo ServiceReceiverInfo { get; set; }
        //public List<Service> Services { get; set; }
        //public List<Location> Locations2 { get; set; }

        public List<int> ServiceLocationId { get; set; }
        public int LocationId { get; set; }
        public int LocationCode { get; set; }
        public int ServiceCode { get; set; }

        public List<int> ServiceId { get; set; }
        public List<int> Locations { get; set; }

        [Display(Name = "حذف منطقی ")]
        public byte Deleted { get; set; }

        public int Id { get; set; }

        public int RoleId { get; set; }

        [Display(Name = "کد کاربر")]
        public int PersonnelId { get; set; }

        [Display(Name = "نام کاربری ")]
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Display(Name = "نام ")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگي")]
        [Required]
        [MaxLength(100)]
        public string Family { get; set; }

        [Display(Name = "کد ملی")]
        [Required]
        public string NationalCode { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "تلفن همراه")]
        [Required]
        [MaxLength(11)]
        public string Mobile { get; set; }

        [Display(Name = "آدرس منزل")]
        [MaxLength(500)]
        public string HomeAddress { get; set; }

        [Display(Name = "استان")]
        [Required]
        public int StateId { get; set; }

        [Display(Name = "شهر")]
        [Required]
        public int CityId { get; set; }

        [Display(Name = "مسیر ذخیره فایل")]
        public string Path { get; set; }
        //[Display(Name = "منطقه")]
        //public string Suburb { get; set; }

        [Display(Name = "جنسیت")]
        [Required]
        public bool Sex { get; set; }

        [Display(Name = "تلفن منزل")]
        [MaxLength(11)]
        public string HomePhone { get; set; }

        [Display(Name = "تاريخ تولد")]
        [MaxLength(10)]
        public string BrithDay { get; set; }

        [Display(Name = "آدرس محل کار")]
        [MaxLength(500)]
        public string WorkAddress { get; set; }

        [Display(Name = "تلفن محل کار")]
        [MaxLength(11)]
        public string WorkPhone { get; set; }

        [Display(Name = "رزومه")]
        [MaxLength(2000)]
        public string Resume { get; set; }

        [Display(Name = "کارت ملی")]
        public string NationalCard { get; set; }

        [Display(Name = "مدرک تحصیلی")]
        public string DegreeEducation { get; set; }

        [Display(Name = "مدرک فنی و حرفه ای")]
        public string Vocational { get; set; }

        [Display(Name = "پلاک")]
        [MaxLength(10)]
        public string HomeNumber { get; set; }

        [Display(Name = "واحد آپارتمان")]
        public int UnitNumber { get; set; }

        [Display(Name = "شماره کارت بانکی")]
        public string BankCardNumber { get; set; }

        [Display(Name = "نحوه انجام خدمات ")]
        public string HowPerformServices { get; set; }

        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Picture { get; set; }

        [Display(Name = "تاریخ عضویت ")]
        public string RegisterDate { get; set; }

        [Display(Name = "کد خدمتیار")]
        public string ServiceProviderCode { get; set; }

        /// <summary>
        /// 0=  خیر
        /// 1=  بلی        
        /// </summary>
        [Display(Name = "وضعیت همکاری")]
        public bool Disconnect { get; set; }

        [Display(Name = "تاریخ قطع همکاری")]
        [MaxLength(10)]
        public string DisconnectDate { get; set; }

        [Display(Name = "دلیل قطع همکاری")]
        [MaxLength(100)]
        public string DisconnectReason { get; set; }

        [Display(Name = "کد مشتری")]
        public string ServiceReceiverCode { get; set; }

        #endregion
    }

}