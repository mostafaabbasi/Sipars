using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceWorkUnits", Schema = "PrivateTraining")]
    public class ServiceWorkUnit:BaseEntity
    {
        [Required]
        public int ServicePropertiesId { get; set; }

        [Required]
        public int WorkUnitId { get; set; }

        [Required]
        public int PriceWorkUnit { get; set; }

        #region Navigatos

        [ForeignKey("ServicePropertiesId")]
        [InverseProperty("ServiceWorkUnits")]
        public virtual ServiceProperties ServiceProperties { get; set; }

        [ForeignKey("WorkUnitId")]
        [InverseProperty("ServiceWorkUnits")]
        public virtual WorkUnit WorkUnits { get; set; }

        #endregion

    }
}
