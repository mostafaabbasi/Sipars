using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("InvalidDayLines", Schema = "BusDriving")]
    public class InvalidDayLine :InvalidDay
    {

        public int LineId { get; set; }

        public int ShiftId { get; set; }

        #region Navigators

        [ForeignKey("LineId")]
        [InverseProperty("InvalidDayLines")]
        public virtual Line Lines { get; set; }

        [ForeignKey("ShiftId")]
        [InverseProperty("InvalidDayLines")]
        public virtual Shift Shifts { get; set; }

        #endregion

    }
}
