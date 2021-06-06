using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class privatetraining_ServiceReceiverRequests
    {
        #region Properties

        [Required]
        public int Id { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string FromTime { get; set; }

        [Required]
        public string ToTime { get; set; }

        [Required]
        public int StatusRequest { get; set; }

        public int PriceReceived { get; set; }

        public int PriceCalcBySystem { get; set; }

        [Required]
        public byte ConfirmServiceReceiver { get; set; }

        public string NextMeeting { get; set; }

        [Required]
        public string FullNameServiceReceiver { get; set; }

        [Required]
        public string FullNameServicProvider { get; set; }

        [Required]
        public decimal ServiceReceiverServiceLocationId { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("ServiceReceiverServiceLocationId")]
        [InverseProperty("ServiceReceiverRequests")]
        public virtual ServiceReceiverServiceLocation ServiceReceiverServiceLocations { get; set; }

        #endregion

    }
}
