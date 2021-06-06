using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("ServiceReceiversInfo", Schema = "Security")]
    public class ServiceReceiverInfo : ApplicationUser
    {
        #region Properties

        [Display(Name = "کد مشتری")]
        public string ServiceReceiverCode { get; set; }

        [Display(Name = "کد محل ")]
        public int LocationCode { get; set; }

        [Display(Name = "کد شخص ")]
        public string UserCode { get; set; }
        
        [Display(Name = "پلاک")]
        [MaxLength(10)]
        public string HomeNumber { get; set; }

        [Display(Name = "واحد آپارتمان")]
        public int UnitNumber { get; set; }
        #endregion

        #region Navigators
     //   public virtual ICollection<ServiceReceiverServiceLocation> ServiceReceiverServiceLocations { get; set; }

        #endregion
    }
}
