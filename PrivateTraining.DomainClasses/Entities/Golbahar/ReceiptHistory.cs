using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Golbahar
{
    [Table("ReceiptHistorys", Schema = "Golbahar")]
    public class ReceiptHistory : BaseEntity
    {

        [Display(Name = "نام ")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگي")]
        [MaxLength(100)]
        public string Family { get; set; }

        [Display(Name = "كد ملي")]
        [MaxLength(10)]
        public string NationalCode { get; set; }

        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string BlocName { get; set; }
        public Nullable<decimal> SumPayUser { get; set; }
        public Nullable<double> SumDeptUser { get; set; }

        [Display(Name = "کدنوسازی")]
        [MaxLength(50)]
        public string RenovationCode { get; set; }

        [Display(Name = "طبقه")]
        [MaxLength(10)]
        public string Floor { get; set; }

        [Display(Name = "شماره واحد")]
        [MaxLength(10)]
        public string UnitNumber { get; set; }

        [Display(Name = "سهم العرصه")]
        public double PortionField { get; set; }

        [Display(Name = "اعیان")]
        public double Area { get; set; }

        [Display(Name = "تاریخ قرارداد")]
        [MaxLength(10)]
        public string ContractDate { get; set; }

        [Display(Name = "مهلت پرداخت")]
        [MaxLength(10)]
        public string PayDate { get; set; }

        [Display(Name = "عرصه کل")]
        public double TotalField { get; set; }

        [Display(Name = "اعیان کل")]
        public double TotalArea { get; set; }

        [Display(Name = "بانک (فیش)")]
        [MaxLength(50)]
        public string ReceiptBankName { get; set; }

        [Display(Name = "شماره حساب (فیش)")]
        [MaxLength(50)]
        public string ReceiptAccountNumber { get; set; }

        [Display(Name = "توضیحات (فیش)")]
        [MaxLength(500)]
        public string ReceiptDesc { get; set; }

        public int UnitId { get; set; }
        public Nullable<double> OldYearPrice { get; set; }
        public Nullable<double> AccountPercentsPay { get; set; }
        public Nullable<int> AccountPercents { get; set; }
        public string BillingID { get; set; }


        public virtual ICollection<ReceiptHistoryYear> ReceiptHistoryYears { get; set; }



    }
}
