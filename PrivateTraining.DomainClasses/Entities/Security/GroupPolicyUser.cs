using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateTraining.DomainClasses.Security;

namespace PrivateTraining.DomainClasses.Entities.Security
{
    [Table("GroupPolicyUsers", Schema = "Security")]
    public class GroupPolicyUser : BaseEntity
    {

        #region Property
        public int UserId { get; set; }
        public int GroupPolicyId { get; set; }
        #endregion

        #region navigation
        [ForeignKey("UserId")]
        [InverseProperty("GroupPolicyUsers")]
        public virtual ApplicationUser ApplicationUsers { get; set; }

        [ForeignKey("GroupPolicyId")]
        [InverseProperty("GroupPolicyUsers")]
        public virtual GroupPolicy GroupPolicies { get; set; }


        #endregion

    }
}
