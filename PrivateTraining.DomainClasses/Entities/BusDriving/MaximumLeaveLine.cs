using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("MaximumLeaveLines", Schema = "BusDriving")]
   public class MaximumLeaveLine :MaximumLeave
    {

        public int LineId { get; set; }

        public int ShiftId { get; set; }


        #region Navigators

        [ForeignKey("LineId")]
        [InverseProperty("MaximumLeaveLines")]
        public virtual Line Lines { get; set; }

        [ForeignKey("ShiftId")]
        [InverseProperty("MaximumLeaveLines")]
        public virtual Shift Shifts { get; set; }

       
       
        #endregion



    }
}
