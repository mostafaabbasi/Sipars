using PrivateTraining.DomainClasses.Entities.Climbings;
using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{
    [Table("PaymentOrderDetails", Schema = "Framework")]

    public class PaymentOrderDetail : BaseEntity
    {

        #region Properties

        [Display(Name = "کد تراکنش ")]
        public int PaymentOrderId { get; set; }

        [Display(Name = " کد برنامه")]
        public Nullable<int> ProgramId { get; set; }

        [Display(Name = "کاربر ثبت کننده")]
        public int UserIdPayment { get; set; }

        [Display(Name = "کاربر شرکت کننده در برنامه")]
        public int RegisterUserId { get; set; }

        [Display(Name = "تاریخ ثبت")]
        [Required]
        [MaxLength(20)]
        public string SaveDate { get; set; }

        [Display(Name = "مبلغ ثبت نام")]
     //   [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal price { get; set; }

        [Display(Name = "جریمه")]
        public decimal Fine { get; set; }

        /// <summary>
        /// 1= پرداخت الکترونیک
        /// 2= idpay 
        /// 3= شارژ پنل 
        /// </summary>
        [Display(Name = "نوع پرداخت")]
        [Required]
        public byte PaymentType { get; set; }

        /// <summary>
        /// 1= دریافتی
        /// 2= برگشتی 
        /// </summary>
        [Display(Name = "نوع تراکنش")]
        [Required]
        public byte TransactionType { get; set; }

        /// <summary>
        /// 1= ثبت نام یا خرید
        /// 2= شارژ پنل 
        /// </summary>
        [Display(Name = "نام تراکنش")]
        [Required]
        public byte TransactionName { get; set; }


        [Display(Name = "تایید مدیر ")]
        [Required]
        public byte ActivePayment { get; set; }

        [Display(Name = "دلیل عدم تایید")]
        public string DesOfReject { get; set; }
        
        [Display(Name = "تاریخ  تایید مدیر")]
        [MaxLength(20)]
        public string SaveDateActivePayment { get; set; }

        [Display(Name = "کنسلی کاربر ")]
        [Required]
        public bool ActivePaymentOfUser { get; set; }

        [Display(Name = "تاریخ کنسلی کاربر")]
        [MaxLength(20)]
        public string SaveDateActivePaymentOfUser { get; set; }

        [Display(Name = "کنسلی برنامه ")]
        [Required]
        public bool CancelProgram { get; set; }

        [Display(Name = "تاریخ کنسلی برنامه")]
        [MaxLength(20)]
        public string SaveDateCancelProgram  { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("ProgramId")]
        [InverseProperty("PaymentOrderDetails")]
        public virtual Program Programs { get; set; }

        //[ForeignKey("PaymentOrderId")]
        //[InverseProperty("PaymentOrderDetails")]
        //public virtual PaymentOrder PaymentOrders { get; set; }


        [ForeignKey("RegisterUserId")]
        [InverseProperty("PaymentOrderDetails")]
        public virtual ApplicationUser RegisterUsers { get; set; }

        #endregion

    }
}
