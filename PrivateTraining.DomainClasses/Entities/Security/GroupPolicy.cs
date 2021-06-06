using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("GroupPolicies", Schema = "Security")]
    public class GroupPolicy : BaseEntity
    {

        #region Property

        [StringLength(300)]
        [Required]
        public string Name { get; set; }

        #endregion

        #region navigation

        public ICollection<GroupPolicyUser> GroupPolicyUsers { get; set; }
        public ICollection<AccessLevelGroup> AccessLevelGroups { get; set; }

        #endregion


    }


}
