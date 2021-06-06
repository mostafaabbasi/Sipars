using PrivateTraining.DomainClasses.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ProblemReturnRegisterPrices", Schema = "PrivateTraining")]
    public  class ProblemReturnRegisterPrice : BaseEntity
    {
       // public byte Type { get; set; }
       // public string Reason { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public Nullable<int> serviceProviderId { get; set; }
        public bool ActiveByAdmin { get; set; }

        // برگشت داده نشده=0 . برگشت داده شده=1 .
        public byte StatusReturnPrice { get; set; }
        public int PriceRegister { get; set; }


        [ForeignKey("serviceProviderId")]
        [InverseProperty("ProblemReturnRegisterPrices")]
        public virtual ServiceProviderInfo ServicesProviderInfo { get; set; }


    }
}
