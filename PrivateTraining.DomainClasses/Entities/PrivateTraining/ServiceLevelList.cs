using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceLevelLists", Schema = "PrivateTraining")]

    public class ServiceLevelList: BaseEntity
    {
        [Required]
        public int ServicePropertiesId { get; set; }

        [Required]
        public int ServiceLevelId { get; set; }

        [Required]
        public int PercentServiceLevel { get; set; }

        #region Navigator

        [ForeignKey("ServicePropertiesId")]
        [InverseProperty("ServiceLevelLists")]
        public virtual ServiceProperties ServiceProperties { get; set; }

        [ForeignKey("ServiceLevelId")]
        [InverseProperty("ServiceLevelLists")]
        public virtual ServiceLevel ServiceLevels { get; set; }

        #endregion
    }
}
