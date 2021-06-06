using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Security
{
    [Table("SupplementaryInfoUser", Schema = "Security")]
    public class SupplementaryInfoUser : ApplicationUser
    {
        //aaa
        #region Property
      


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


        

        #endregion

    }
}
