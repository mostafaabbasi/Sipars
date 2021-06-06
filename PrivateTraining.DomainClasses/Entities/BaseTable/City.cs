using PrivateTraining.DomainClasses.Entities.Climbings;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.DomainClasses.Entities.BaseTable
{
    [Table("Cities", Schema = "BaseInfo")]
    public class City : BaseEntity
    {
        #region Property
        public string Name { get; set; }
        public int StateId { get; set; }

        [NotMapped]
        public bool selected { get; set; }

        #endregion

        #region nvaigation

        [ForeignKey("StateId")]
        [InverseProperty("Cities")]
        public virtual State States { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<View_ServiceLocations> ServiceLocations { get; set; }

        public virtual ICollection<Club> Clubs { get; set; }
        public virtual ICollection<Program> Programs { get; set; }

        #endregion

    }
}
