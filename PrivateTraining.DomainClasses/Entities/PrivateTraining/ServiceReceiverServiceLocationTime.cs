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
    [Table("ServiceReceiverServiceLocationTimes", Schema = "PrivateTraining")]
    public class ServiceReceiverServiceLocationTime : ServiceReceiverServiceLocation
    {
        #region Properties

        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int StatusRequest { get; set; }
        public int Price { get; set; }
       
        #endregion

    

    }
}
