using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.Climbings
{
    [Table("ProgramModrators", Schema = "Climbing")]

    public class ProgramModrator : BaseEntity
    {
        #region Properties

        public int UserId { get; set; }
        public int ProgramId { get; set; }
        public string SaveDate { get; set; }
        public string UpdateDate { get; set; }

        #endregion

        #region Navigators

        [ForeignKey("UserId")]
        [InverseProperty("ProgramModrators")]
        public virtual ApplicationUser Modrators { get; set; }

        [ForeignKey("ProgramId")]
        [InverseProperty("ProgramModrators")]
        public virtual Program ProgramTable { get; set; }

        #endregion
    }
}
