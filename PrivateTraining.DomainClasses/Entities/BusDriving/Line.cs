using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("Lines", Schema = "BusDriving")]
    public class Line : BaseEntity
    {
        /// <summary>
        /// شماره خط
        /// </summary>
        [StringLength(300)]
        [Required]
        public string Number { get; set; }

        /// <summary>
        /// نام خط
        /// </summary>
        [StringLength(300)]
        [Required]
        public string Name { get; set; }

        #region Navigatos

        public virtual ICollection<MaximumLeaveLine> MaximumLeaveLines { get; set; }
        public virtual ICollection<InvalidDayLine> InvalidDayLines { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

        #endregion
    }
}
