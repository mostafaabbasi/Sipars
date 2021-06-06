using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Security;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("AccessLevelUsers", Schema = "Security")]
    public class AccessLevelUser : AccessLevel
    {

        #region Property

        public int UserId { get; set; }

        #endregion

        #region navigation

        [ForeignKey("UserId")]
        [InverseProperty("AccessLevelUsers")]
        public virtual ApplicationUser ApplicationUsers { get; set; }

        #endregion
    }
}
