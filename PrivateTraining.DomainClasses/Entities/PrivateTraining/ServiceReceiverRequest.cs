using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceReceiverRequests", Schema = "PrivateTraining")]
    public class ServiceReceiverRequest : BaseEntity
    {
        #region Properties
        [Required]
        public string Date { get; set; }

        [Required]
        public string FromTime { get; set; }

        [Required]
        public string ToTime { get; set; }

        //موافق | مخالف | قطعی | ناتمام | تمام |
        [Required]
        public int StatusRequest { get; set; }

        public int PriceReceived { get; set; }

        public int PriceCalcBySystem { get; set; }

        [Required]
        public byte ConfirmServiceReceiver { get; set; }

        public string NextMeeting { get; set; }

        [Required]
        public decimal ServiceReceiverServiceLocationId { get; set; }

       [MaxLength(10)]
        public string PriceReceivedDate { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("ServiceReceiverServiceLocationId")]
        [InverseProperty("ServiceReceiverRequests")]
        public virtual ServiceReceiverServiceLocation ServiceReceiverServiceLocations { get; set; }

        #endregion

    }
}
