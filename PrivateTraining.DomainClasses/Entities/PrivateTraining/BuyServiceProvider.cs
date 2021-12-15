using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    public class BuyServiceProvider
    {
        public int Id { get; set; }
        public int BuyServiceId { get; set; }
        public int CustomerId { get; set; }
        public int ProviderId { get; set; }
        public int RequestState { get; set; }
        public DateTime? CreateAtRequestState { get; set; }
        public bool State  { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
