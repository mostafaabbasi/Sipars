using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("ServicesProviderInfo", Schema = "Security")]
    public class ServiceProviderInfo : ApplicationUser
    {
        #region Properties

        [Display(Name = "کد خدمت دهنده")]
        public string ServiceProviderCode { get; set; }

        [Display(Name = "تعداد ستاره های خدمت دهنده ")]
        public int StarScore { get; set; }

        [Display(Name = "کد خدمت ")]
        public int ServiceCode { get; set; }

        [Display(Name = "کد محل ")]
        public int LocationCode { get; set; }

        [Display(Name = "کد شخص ")]
        public string UserCode { get; set; }

        //[Display(Name = "تاريخ تولد")]
        //[MaxLength(10)]
        //public string BrithDay { get; set; }

        [Display(Name = "آدرس محل کار")]
        [MaxLength(500)]
        public string WorkAddress { get; set; }

        [Display(Name = "تلفن محل کار")]
        [MaxLength(11)]
        public string WorkPhone { get; set; }

        [Display(Name = "رزومه")]
        [MaxLength(2000)]
        public string Resume { get; set; }

        //[Display(Name = "کارت ملی")]
        //public string NationalCard { get; set; }

        //[Display(Name = "مدرک تحصیلی")]
        //public string DegreeEducation { get; set; }

        //[Display(Name = "مدرک فنی و حرفه ای")]
        //public string Vocational { get; set; }

        //[Display(Name = "مسیر ذخیره فایل ")]
        //public string PathDocumentations { get; set; }

        [Display(Name = "نحوه انجام خدمات ")]
        public string HowPerformServices { get; set; }

        [Display(Name = "شماره کارت بانکی")]
        public string BankCardNumber { get; set; }


        /// <summary>
        /// سطح عادی =0 و سطح ویزه =1
        /// </summary>
        [Display(Name = "سطح کاربر")]
        public byte Level { get; set; }

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

        #endregion

        #region Navigators

        public virtual ICollection<DebtServiceProvider> DebtServiceProviders { get; set; }
        public virtual ICollection<ProblemReturnRegisterPrice> ProblemReturnRegisterPrices { get; set; }
        public virtual ICollection<ServiceReceiverServiceLocation> ServiceReceiverServiceLocations { get; set; }

        #endregion
    }
}
