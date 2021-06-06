using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Climbings
{
    [Table("ClubManagers", Schema = "Climbing")]

    public class ClubManager : BaseEntity
    {
        #region Properties

        public int UserId { get; set; }
        public int ClubId { get; set; }
        public string SaveDate { get; set; }
        public string UpdateDate { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("UserId")]
        [InverseProperty("ClubManagers")]
        public virtual ApplicationUser UserClub { get; set; }

        [ForeignKey("ClubId")]
        [InverseProperty("ClubManagers")]
        public virtual Club ClubTable { get; set; }

        #endregion
    }
}
