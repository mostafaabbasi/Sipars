using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceNotInLocations", Schema = "PrivateTraining")]
    public class ServiceNotInLocation : BaseEntity
    {
        #region Properties

        public int LocationId { get; set; }
        public int ServiceId { get; set; }

        #endregion

        #region Navigators

        //[ForeignKey("LocationId")]
        //[InverseProperty("ServiceNotInLocations")]
        //public virtual Location Locations { get; set; }

        //[ForeignKey("ServiceId")]
        //[InverseProperty("ServiceNotInLocations")]
        //public virtual Service Services { get; set; }

        #endregion


    }
}
