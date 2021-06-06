using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("AccessLevelGroups", Schema = "Security")]
    public class AccessLevelGroup : AccessLevel
    {

        #region Property

        public int GroupId { get; set; }

        #endregion

        #region navigation

        [ForeignKey("GroupId")]
        [InverseProperty("AccessLevelGroups")]
        public virtual GroupPolicy GroupPolicies { get; set; }

        #endregion
    }
}
