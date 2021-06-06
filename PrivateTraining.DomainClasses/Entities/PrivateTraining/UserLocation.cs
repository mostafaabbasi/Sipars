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
    [Table("UserLocations", Schema = "PrivateTraining")]
    public class UserLocation: BaseEntity
    {
        #region Properties
        [Required]
        public int UserId { get; set; }

        [Required]
        public int LocationId { get; set; }
        #endregion

        #region Navigators
        [ForeignKey("UserId")]
        [InverseProperty("UserLocations")]
        public virtual ApplicationUser Users { get; set; }


        [ForeignKey("LocationId")]
        [InverseProperty("UserLocations")]
        public virtual Location Locations { get; set; }
        #endregion
    }
}
