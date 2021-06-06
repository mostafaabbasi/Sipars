using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("payments", Schema = "PrivateTraining")]
    public class payment : BaseEntity
    {
        public int Price { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public int TransactionNumber { get; set; }

        // تایید شده 1 ,  رد شده 0
        public byte Status { get; set; }

        public int MemberId { get; set; }

        public int ModratorId { get; set; }

        public string CodeBank { get; set; }

        public int DeptId { get; set; }

        public int verified { get; set; }

        public byte ActivePayment { get; set; }

        [ForeignKey("MemberId")]
        [InverseProperty("payments")]
        public virtual ApplicationUser Members { get; set; }

        [ForeignKey("ModratorId")]
        public virtual ApplicationUser Modrators { get; set; }

        public virtual ICollection<paymentDetail> paymentDetails { get; set; }

    }
}
