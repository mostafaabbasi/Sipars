using PrivateTraining.DomainClasses.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("DebtServiceProviders", Schema = "PrivateTraining")]
    public class DebtServiceProvider : Debt
    {
        public int ServiceProviderId { get; set; }

        [ForeignKey("ServiceProviderId")]
        [InverseProperty("DebtServiceProviders")]
        public virtual ServiceProviderInfo ServicesProviders { get; set; }
    }
}
