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

    [Table("PaymentOrders", Schema = "Framework")]
    public class PaymentOrder : BaseEntity
    {
        #region Properties

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


        [Display(Name = "کاربر ثبت کننده")]
        public int UserIdPayment { get; set; }

        //[Display(Name = "کاربر شرکت کننده در برنامه")]
        //public int RegisterUserId { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        [Required]
        //    [MaxLength(20)]
        public string SaveDatePayment { get; set; }

        [Display(Name = "مبلغ کل")]
        [Required]
        public decimal AllPrice { get; set; }

        //[Display(Name = "مبلغ جزء")]
        //[Required]
        //public int Price { get; set; }

        [Display(Name = "مبلغ استفاده از پنل")]
        public decimal CalculatePanel { get; set; }

        [Display(Name = "مبلغ استفاده از پرداخت آنلاین")]
        public decimal CalculateOnline { get; set; }

        [Display(Name = "کد رهگیری")]
        //  [Required]
        //  [MaxLength(100)]
        public string TransactionCode { get; set; }

        /// <summary>
        /// در صورتی که از سایت idpay استفاده کرده
        /// </summary>
        [Display(Name = "رسید دیجیتالی")]
        // [MaxLength(100)]
        public string DigitalReceipt { get; set; }

        [Display(Name = "شماره سفارش")]
        [Required]
        public int SaleOrderId { get; set; }

        [Display(Name = "")]
        // [MaxLength(100)]
        //  [Required]
        public string SaleReferenceId { get; set; }

        [Display(Name = "")]
        // [Required]
        // [MaxLength(100)]
        public string CodeBank { get; set; }

        //0 تراکنش ثبت شده ولی سمت بانک نرفته است
        //1 تراکنش توسط بانک تایید شده است
        //2 تراکنش تایید نشده است
        //3 فیش بانکی ثبت شده است
        public int verified { get; set; }

        public string ReturnValueBank { get; set; }

        /// <summary>
        /// در برنامه گلبهار شماره واحد است
        /// </summary>
        [Display(Name = "شماره برنامه")]
        public Nullable<int> OrderId { get; set; }

        #endregion

        #region Navigatos

        [ForeignKey("UserIdPayment")]
        [InverseProperty("PaymentOrders")]
        public virtual ApplicationUser Users { get; set; }

        //[ForeignKey("RegisterUserId")]
        //public virtual ApplicationUser RUsers { get; set; }

       // public virtual ICollection<PaymentOrderDetail> PaymentOrderDetails { get; set; }

        #endregion

    }
}
