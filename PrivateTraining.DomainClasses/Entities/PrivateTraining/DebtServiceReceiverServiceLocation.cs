using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("DebtServiceReceiverServiceLocations", Schema = "PrivateTraining")]
    public class DebtServiceReceiverServiceLocation : Debt
    {
        public decimal ServiceReceiverServiceLocationId { get; set; }

        [ForeignKey("ServiceReceiverServiceLocationId")]
        [InverseProperty("DebtServiceReceiverServiceLocations")]
        public virtual ServiceReceiverServiceLocation ServiceReceiverServiceLocations { get; set; }
    }
}
