using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("Debts", Schema="PrivateTraining")]
    public class Debt : BaseEntity
    {

        [Display(Name = "مبلغ واحد کار")]
        public double TotalCostUnit { get; set; }

        [Display(Name = "مبلغ کل")]
        public double TotalCost { get; set; }

        [Display(Name = "مبلغ کل دریافتی")]
        public double TotalCostReceived { get; set; }

        [Display(Name = "درصد حق السهم شرکت")]
        public int PercentOfShares { get; set; }

        [Display(Name = "مبلغ پرداختی به شرکت")]
        public double CompanyCost { get; set; }

        [Display(Name = "تاریخ")]
        public string Date { get; set; }

        //پرداخت شده 1 - پرداخت نشده 0
        public byte Status { get; set; }
        //موافق=1 و قطعی =2 و ناتمام =3 واتمام=4و درحال بررسی=0 و غیرقطعی=6
        public byte StatusServiceReceiverServiceLocation { get; set; }

        public virtual ICollection<paymentDetail> PaymentDetails { get; set; }

    }
}
