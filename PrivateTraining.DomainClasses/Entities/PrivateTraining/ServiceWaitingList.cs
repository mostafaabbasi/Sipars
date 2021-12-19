using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{

    [Table("ServiceWaitingList", Schema = "PrivateTraining")]
    public class ServiceWaitingList : BaseEntity
    {
        public int BuyServiceId { get; set; }
        public int serviceProviderId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPending { get; set; }
        public bool IsAccept { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
