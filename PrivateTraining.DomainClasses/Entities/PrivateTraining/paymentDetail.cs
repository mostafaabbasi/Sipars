using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("paymentDetails", Schema = "PrivateTraining")]
    public class paymentDetail:BaseEntity
    {
        public int paymentId { get; set; }
        public int DebtId { get; set; }
        //public int Price { get; set; }
        public int? ServiceProviderId { get; set; }
        public int? ServiceReceiverServiceLocationId { get; set; }
        public int MemberId { get; set; }
        public int ModratorId { get; set; }
        public byte Status { get; set; }
        public int CalculatePricePayment { get; set; }
        public double TotalCostDebt { get; set; }
        public int PercentOfSharesDebt { get; set; }
        public double CompanyCostDebt { get; set; }
        public string Date { get; set; }

        [ForeignKey("paymentId")]
        [InverseProperty("paymentDetails")]
        public virtual payment Payments { get; set; }

        [ForeignKey("DebtId")]
        [InverseProperty("paymentDetails")]
        public virtual Debt Debt { get; set; }

        [ForeignKey("MemberId")]
        //[InverseProperty("paymentDetails")]
        public virtual ApplicationUser Members { get; set; }

        [ForeignKey("ModratorId")]
        public virtual ApplicationUser Modrators { get; set; }


    }
}
