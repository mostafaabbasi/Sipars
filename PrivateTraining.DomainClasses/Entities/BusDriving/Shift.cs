using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("Shifts", Schema = "BusDriving")]
    public class Shift : BaseEntity
    {
        /// <summary>
        /// نام شیفت
        /// </summary>
        [StringLength(300)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "از ساعت")]
        [Required]
        //[MaxLength(10)]
        //[MinLength(10)]
        public string FromTime { get; set; }

        [Display(Name = "تا ساعت")]
        [Required]
        //[MaxLength(10)]
        //[MinLength(10)]
        public string ToTime { get; set; }

        #region Navigatos

        public virtual ICollection<MaximumLeaveLine> MaximumLeaveLines { get; set; }
        public virtual ICollection<InvalidDayLine> InvalidDayLines { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

        #endregion
    }
}
